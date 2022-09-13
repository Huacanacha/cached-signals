namespace Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;
    using huacanacha.signal;

    public class CachedSignalTest
    {
        [Test]
        public void ZeroParameterTest() {
            var signal = new CachedSignal();

            bool calledMe = false;
            System.Action callMe = () => {calledMe = true;};
            System.Action dontCallMe = () => {Assert.Fail();};

            // Before firing signal
            var dontCallMeReceipt = signal.Subscribe(dontCallMe);
            var callMeReceipt = signal.Subscribe(callMe);
            Assert.IsFalse(signal.HasValue);
            Assert.IsFalse(calledMe);

            // After firing signal
            // signal.UnsubscribeByCallback(dontCallMe);
            dontCallMeReceipt.Unsubscribe();
            signal.Send();
            Assert.IsTrue(signal.HasValue);
            Assert.IsTrue(calledMe);

            // After removing listener & firing
            calledMe = false;
            callMeReceipt.Unsubscribe();
            // signal.UnsubscribeByCallback(callMe);
            signal.Send();
            Assert.IsFalse(calledMe);

            // Test that cached signal is fired on AddListener
            bool calledMeNow = false;
            System.Action callMeNow = () => {calledMeNow = true;};
            signal.Subscribe(callMeNow);
            Assert.IsTrue(calledMeNow);

            // After clearing fired signal
            signal.ClearCache();
            Assert.IsFalse(signal.HasValue);
        }

        [Test]
        public void ZeroParameterAddOnceTest() {
            // *** Case A - add then dispatch ***
            var signal = new CachedSignal();
            bool aaa = false;
            System.Action callbackA = () => {aaa = true;};

            // Before firing signal
            signal.SubscribeOnce(callbackA);
            Assert.IsFalse(aaa);
            Assert.IsFalse(signal.HasValue);

            // After firing signal
            signal.Send();
            Assert.IsTrue(aaa);

            // After second firing - listener should be removed
            aaa = false;
            signal.Send();
            Assert.IsFalse(aaa);

            // *** Case B - add listener after signal has been dispatched ***
            signal = new CachedSignal();
            bool bbb = false;
            System.Action callbackB = () => {bbb = true;};

            Assert.IsFalse(bbb);
            signal.Send();
            signal.SubscribeOnce(callbackB);
            Assert.IsTrue(bbb);

            // Now check that the listener wasn't added
            bbb = false;
            Assert.IsFalse(bbb);
            signal.Send();
            Assert.IsFalse(bbb);

            // *** Case C - multiple listeners ***
            signal = new CachedSignal();
            aaa = false;
            bbb = false;

            signal.SubscribeOnce(callbackA);
            signal.SubscribeOnce(callbackB);
            Assert.IsFalse(aaa);
            Assert.IsFalse(bbb);

            signal.Send();

            Assert.IsTrue(aaa);
            Assert.IsTrue(bbb);

            // Case D - unsubscribe during callback... fancy!
            signal = new CachedSignal();
            var unsubscriber = new UnsubscribeInCallback(signal);

            Assert.IsFalse(unsubscriber.Called);

            Assert.DoesNotThrow(() => {
                signal.Send();
            });

            Assert.IsTrue(unsubscriber.Called);
        }

        [Test]
        public void ZeroParameterRecursion() {
            var signal = new CachedSignal();
            bool l1 = false;
            bool l2 = false;

            var receipt = signal.Subscribe(() => {
                l1 = true;
                signal.SubscribeOnce(() => {
                    l2 = true;
                });
            });

            Assert.DoesNotThrow(() => signal.Send());

            Assert.IsTrue(l1);
            Assert.IsTrue(l2);

            signal = new CachedSignal();
            int i1 = 0;
            int i2 = 0;
            int i3 = 0;
            int i4 = 0;
            int i5 = 0;

            receipt = signal.Subscribe(() => {
                ++i4;
            });

            signal.Subscribe(() => {
                receipt.Unsubscribe();
                ++i1;
                SubscriptionReceipt? sr = null;
                if (i1 == 1) {
                    signal.SubscribeOnce(() => {
                        ++i2;
                    });
                    sr = signal.Subscribe(() => {
                        ++i5;
                    });
                }
                signal.Send(); // Signal system prevents re-sending withing send callbacks
                sr?.Unsubscribe();
            });

            // Send twice
            Assert.DoesNotThrow(() =>
                signal.Send()
            );
            signal.Send();

            Assert.AreEqual(2, i1);
            Assert.AreEqual(1, i2);
            Assert.AreEqual(1, i4);
            Assert.AreEqual(2, i5); // Once cached, once on outer Send

            signal = new CachedSignal();
            signal.Subscribe(() => {
                ++i3;
            });

            signal.Send();

            Assert.AreEqual(2, i1);
            Assert.AreEqual(1, i2);
            Assert.AreEqual(1, i3);
        }

        [Test]
        public void OneParameterTest() {
            var signal = new CachedSignal<int>();

            bool calledMe = false;
            int setMeInt = -1;
            System.Action<int> setMe = (a) => {calledMe = true; setMeInt = a;};
            System.Action<int> dontCallMe = (a) => {Assert.Fail();};

            // Before firing signal
            signal.Subscribe(setMe);
            signal.Subscribe(dontCallMe);
            Assert.IsFalse(signal.HasValue);
            Assert.IsFalse(calledMe);

            // After firing signal
            signal.UnsubscribeByCallback(dontCallMe);
            signal.Send(1);
            Assert.IsTrue(signal.HasValue);
            Assert.IsTrue(calledMe);
            Assert.AreEqual(1, setMeInt);

            // After firing signal again
            signal.Send(42);
            Assert.AreEqual(42, setMeInt);

            // After removing listener & firing
            calledMe = false;
            signal.UnsubscribeByCallback(setMe);
            signal.Send(77);
            Assert.IsFalse(calledMe);
            Assert.AreNotEqual(77, setMeInt);

            // Test that cached signal is fired on AddListener
            signal.Send(144);
            int setMeNowInt = -1;
            System.Action<int> callMeNow = (a) => {setMeNowInt = a;};
            signal.Subscribe(callMeNow);
            Assert.AreEqual(144, setMeNowInt);

            // After clearing fired signal
            signal.ClearCache();
            Assert.IsFalse(signal.HasValue);

            // Case D - unsubscribe during callback... fancy!
            signal.ClearCache();
            var unsubscriber = new UnsubscribeInCallback<int>(signal);

            Assert.AreEqual(0, unsubscriber.Value);

            Assert.DoesNotThrow(() => {
                signal.Send(999);
            });
            Assert.AreEqual(999, unsubscriber.Value);
        }

        [Test]
        public void OneParameterHasListenersSignalTest() {
            var signal = new Signal<int>();
            var cachedSignal = new CachedSignal<int>();

            int setMeInt = -100;
            int cached_setMeInt = -100;
            System.Action<int> setMe = (a) => {setMeInt = a;};
            System.Action<int> cached_setMe = (a) => {cached_setMeInt = a;};

            Assert.IsFalse(signal.HasListeners);
            Assert.IsTrue(cachedSignal.HasListeners); // Always true for CachedSignals

            // Signals with no listeners should not run the Send code
            if (signal.HasListeners) {
                ++setMeInt;
                signal.Send(888);
            }
            // CachedSignals should always run the Send code, as they cache the value and HasListeners should always be true
            if (cachedSignal.HasListeners) {
                ++cached_setMeInt;
                cachedSignal.Send(555);
            }

            Assert.AreEqual(-100, setMeInt);
            Assert.AreEqual(-99, cached_setMeInt);
            Assert.IsTrue(cachedSignal.HasValue);
            Assert.AreEqual(555, cachedSignal.Value);

            var sub = signal.Subscribe(setMe);
            var sub2 = cachedSignal.Subscribe(cached_setMe);
            Assert.IsTrue(signal.HasListeners);
            Assert.IsTrue(cachedSignal.HasListeners);

            // Signal and CachedSignal should both run the Send code
            if (signal.HasListeners) {
                signal.Send(999);
            }
            if (cachedSignal.HasListeners) {
                cachedSignal.Send(444);
            }

            Assert.IsTrue(cachedSignal.HasValue);
            Assert.AreEqual(999, setMeInt);
            Assert.AreEqual(444, cached_setMeInt);

            sub.Unsubscribe();
            sub2.Unsubscribe();
            Assert.IsFalse(signal.HasListeners);
            Assert.IsTrue(cachedSignal.HasListeners);

            // Same same as above but using SendIfListeners method
            var sig2 = new Signal<int>();
            var cachedSig2 = new CachedSignal<int>();

            int s_someNumber = 20;
            int cs_someNumber = 50;
            System.Func<int> s_expensiveWorkAction = () => {
                // Do expensive work
                ++s_someNumber;
                return s_someNumber;
            };
            System.Func<int> cs_expensiveWorkAction = () => {++cs_someNumber; return cs_someNumber;};

            Assert.IsFalse(sig2.HasListeners);
            Assert.IsTrue(cachedSig2.HasListeners);
            Assert.AreEqual(20, s_someNumber);
            Assert.AreEqual(50, cs_someNumber);

            sig2.SendIfListeners(s_expensiveWorkAction);
            cachedSig2.SendIfListeners(cs_expensiveWorkAction);
            Assert.AreEqual(20, s_someNumber); // No change because no listeners, so action doesn't run
            Assert.AreEqual(51, cs_someNumber); // CachedSignals always Send, so +1 the value

            var s_sub = sig2.Subscribe((a) => {
                Assert.AreEqual(s_someNumber, a);
                s_someNumber += 100;
            });
            var cs_sub = cachedSig2.Subscribe((a) => {
                Assert.AreEqual(cs_someNumber, a);
                cs_someNumber += 100;
            });
            Assert.IsTrue(sig2.HasListeners);
            Assert.IsTrue(cachedSig2.HasListeners);
            Assert.AreEqual(151, cs_someNumber); // CachedSignals will call on Subscribe, but without redoing the "work"

            sig2.SendIfListeners(s_expensiveWorkAction);
            cachedSig2.SendIfListeners(cs_expensiveWorkAction);
            Assert.AreEqual(121, s_someNumber); // Action ran, so number changed from send Func (+1) and from the listener logic (+100)
            Assert.AreEqual(252, cs_someNumber); // CachedSignals always Send, so +1 and +100

            s_sub.Unsubscribe();
            cs_sub.Unsubscribe();
            sig2.SendIfListeners(s_expensiveWorkAction);
            cachedSig2.SendIfListeners(cs_expensiveWorkAction);
            Assert.AreEqual(121, s_someNumber); // No change because no listeners, so action doesn't run
            Assert.AreEqual(253, cs_someNumber); // CachedSignals always Send, so +1 the value
            Assert.IsFalse(sig2.HasListeners);
            Assert.IsTrue(cachedSig2.HasListeners);
        }

        [Test]
        public void OneParameterStringTest() {
            var signal = new CachedSignal<string>();

            bool calledMe = false;
            string setMeString = "-1";
            System.Action<string> setMe = (a) => {calledMe = true; setMeString = a;};
            System.Action<string> dontCallMe = (a) => {Assert.Fail();};

            // Before firing signal
            signal.Subscribe(setMe);
            signal.Subscribe(dontCallMe);
            Assert.IsFalse(signal.HasValue);
            Assert.IsFalse(calledMe);

            // After firing signal
            signal.UnsubscribeByCallback(dontCallMe);
            signal.Send("1");
            Assert.IsTrue(signal.HasValue);
            Assert.IsTrue(calledMe);
            Assert.AreEqual("1", setMeString);

            // After firing signal again
            signal.Send("42");
            Assert.AreEqual("42", setMeString);

            // After removing listener & firing
            calledMe = false;
            signal.UnsubscribeByCallback(setMe);
            signal.Send("77");
            Assert.IsFalse(calledMe);
            Assert.AreNotEqual("77", setMeString);

            // Test that cached signal is fired on AddListener
            signal.Send("144");
            string setMeNowString = "-1";
            System.Action<string> callMeNow = (a) => {setMeNowString = a;};
            signal.Subscribe(callMeNow);
            Assert.AreEqual("144", setMeNowString);

            // After clearing fired signal
            signal.ClearCache();
            Assert.IsFalse(signal.HasValue);
        }

        [Test]
        public void OneParameterSendIfChangedTest() {
            var signal = new CachedSignal<int>();
            int count = 0;
            signal.Subscribe((_) => {
                ++count;
            });

            Assert.AreEqual(0, count);
            Assert.IsFalse(signal.HasValue);

            // Does send - first time always sends
            var result = signal.SendIfChanged(111);
            Assert.IsTrue(result);
            Assert.AreEqual(1, count);
            Assert.AreEqual(111, signal.Value);
            Assert.IsTrue(signal.HasValue);

            // Does NOT send - value is the same as cached
            result = signal.SendIfChanged(111);
            Assert.IsFalse(result);
            Assert.AreEqual(1, count);
            Assert.AreEqual(111, signal.Value);
            Assert.IsTrue(signal.HasValue);

            // Does send - value is different from cached
            result = signal.SendIfChanged(222);
            Assert.IsTrue(result);
            Assert.AreEqual(2, count);
            Assert.AreEqual(222, signal.Value);
            Assert.IsTrue(signal.HasValue);

            var refSignal = new CachedSignal<SimpleClass>();
            var a = new SimpleClass(333);
            var b = new SimpleClass(444);

            count = 0;
            refSignal.Subscribe((_) => {
                ++count;
            });

            Assert.AreEqual(0, count);
            Assert.IsFalse(refSignal.HasValue);

            refSignal.SendIfChanged(null);
            Assert.AreEqual(1, count);
            Assert.IsTrue(refSignal.HasValue);
            Assert.AreEqual(null, refSignal.Value);

            refSignal.SendIfChanged(null);
            Assert.AreEqual(1, count);

            refSignal.SendIfChanged(a);
            Assert.AreEqual(2, count);
            Assert.AreEqual(a, refSignal.Value);

            refSignal.SendIfChanged(a);
            Assert.AreEqual(2, count);
            Assert.AreEqual(a, refSignal.Value);

            refSignal.SendIfChanged(b);
            Assert.AreEqual(3, count);
            Assert.AreEqual(b, refSignal.Value);

            refSignal.SendIfChanged(null);
            Assert.AreEqual(4, count);
            Assert.AreEqual(null, refSignal.Value);
        }

        class SimpleClass {
            int Value;
            public SimpleClass(int value) => Value = value;
        }

        [Test]
        public void OneParameterReferenceTest() {
            var signal = new CachedSignal<SimpleClass>();

            bool calledMe = false;
            SimpleClass setMeValue = null;
            System.Action<SimpleClass> setMe = (a) => {calledMe = true; setMeValue = a;};
            System.Action<SimpleClass> dontCallMe = (a) => {Assert.Fail();};

            // Before firing signal
            signal.Subscribe(setMe);
            signal.Subscribe(dontCallMe);
            Assert.IsFalse(signal.HasValue);
            Assert.IsFalse(calledMe);

            // After firing signal
            var value1 = new SimpleClass(1);
            var altValue1 = new SimpleClass(1);
            signal.UnsubscribeByCallback(dontCallMe);
            signal.Send(value1);
            Assert.IsTrue(signal.HasValue);
            Assert.IsTrue(calledMe);
            Assert.AreEqual(value1, setMeValue); // Same object
            Assert.AreNotEqual(altValue1, setMeValue); // Value is the same, reference/object is different

            // After firing signal again
            var value2 = new SimpleClass(42);
            signal.Send(value2);
            Assert.AreEqual(value2, setMeValue);
            Assert.AreNotEqual(value1, setMeValue);

            // After removing listener & firing
            var value3 = new SimpleClass(77);
            calledMe = false;
            signal.UnsubscribeByCallback(setMe);
            signal.Send(value3);
            Assert.IsFalse(calledMe);
            Assert.AreNotEqual(value3, setMeValue);

            // Test that cached signal is fired on AddListener
            var value4 = new SimpleClass(144);
            signal.Send(value4);
            SimpleClass setMeNowValue = null;
            System.Action<SimpleClass> callMeNow = (a) => {setMeNowValue = a;};
            signal.Subscribe(callMeNow);
            Assert.AreEqual(value4, setMeNowValue);

            // After clearing fired signal
            signal.ClearCache();
            Assert.IsFalse(signal.HasValue);
        }

        [Test]
        public void OneParameterAddOnceTest() {
            // *** Case A - add then dispatch ***
            var signal = new CachedSignal<int>();
            int aaa = -1;
            System.Action<int> callbackA = (value) => {aaa = value;};

            // Before firing signal
            signal.SubscribeOnce(callbackA);
            Assert.AreEqual(-1, aaa);
            Assert.IsFalse(signal.HasValue);

            // After firing signal
            signal.Send(3);
            Assert.AreEqual(3, aaa);

            // After second firing - listener should be removed
            aaa = -1;
            signal.Send(5);
            Assert.AreEqual(-1, aaa);

            // *** Case B - add listener after signal has been dispatched ***
            signal = new CachedSignal<int>();
            int bbb = -1;
            System.Action<int> callbackB = (value) => {bbb = value;};

            Assert.AreEqual(-1, bbb);
            signal.Send(7);
            signal.SubscribeOnce(callbackB);
            Assert.AreEqual(7, bbb);

            // Now check that the listener wasn't added to fire again
            bbb = -1;
            Assert.AreEqual(-1, bbb);
            signal.Send(9);
            Assert.AreEqual(-1, bbb);

            // *** Case C - multiple listeners ***
            signal = new CachedSignal<int>();
            aaa = -1;
            bbb = -1;

            signal.SubscribeOnce(callbackA);
            signal.SubscribeOnce(callbackB);
            Assert.AreEqual(-1, aaa);
            Assert.AreEqual(-1, bbb);

            signal.Send(42);
            Assert.AreEqual(42, aaa);
            Assert.AreEqual(42, bbb);

            // *** Case C - AddOnce after clearing ***
            signal = new CachedSignal<int>();
            aaa = -1;
            bbb = -1;

            signal.SubscribeOnce(callbackA);
            Assert.AreEqual(-1, aaa);
            Assert.AreEqual(-1, bbb);

            signal.Send(66);
            Assert.AreEqual(66, aaa);
            Assert.AreEqual(-1, bbb);

            signal.ClearCache();
            signal.SubscribeOnce(callbackB);
            Assert.AreEqual(66, aaa);
            Assert.AreEqual(-1, bbb);

            signal.Send(77);
            Assert.AreEqual(66, aaa);
            Assert.AreEqual(77, bbb);
        }

        [Test]
        public void TwoParameterTest() {
            var signal = new CachedSignal<int,int>();

            bool calledMe = false;
            var setMeInts = (-1,-1);
            System.Action<int,int> setMe = (a,b) => {calledMe = true; setMeInts = (a,b);};
            System.Action<int,int> dontCallMe = (a,b) => {Assert.Fail();};

            // Before firing signal
            signal.Subscribe(setMe);
            signal.Subscribe(dontCallMe);
            Assert.IsFalse(signal.HasValue);
            Assert.IsFalse(calledMe);

            // After firing signal
            signal.UnsubscribeByCallback(dontCallMe);
            signal.Send(1,2);
            Assert.IsTrue(signal.HasValue);
            Assert.IsTrue(calledMe);
            Assert.AreEqual((1,2), setMeInts);

            // After firing signal again
            signal.Send(42,43);
            Assert.AreEqual((42,43), setMeInts);

            // After removing listener & firing
            calledMe = false;
            signal.UnsubscribeByCallback(setMe);
            signal.Send(77,78);
            Assert.IsFalse(calledMe);
            Assert.AreNotEqual((77,78), setMeInts);

            // Test that cached signal is fired on AddListener
            signal.Send(144,145);
            var setMeNowInts = (-1,-1);
            System.Action<int,int> callMeNow = (a,b) => {setMeNowInts = (a,b);};
            signal.Subscribe(callMeNow);
            Assert.AreEqual((144,145), setMeNowInts);

            // After clearing fired signal
            signal.ClearCache();
            Assert.IsFalse(signal.HasValue);
        }

        [Test]
        public void TwoParameterSendIfChangedTest() {
            var signal = new CachedSignal<int, SimpleClass>();
            int count = 0;
            signal.Subscribe((_, _) => {
                ++count;
            });

            Assert.AreEqual(0, count);
            Assert.IsFalse(signal.HasValue);

            // Does send - first time always sends
            var result = signal.SendIfChanged(111, null);
            Assert.IsTrue(result);
            Assert.AreEqual(1, count);
            Assert.AreEqual(System.ValueTuple.Create<int, SimpleClass>(111, null), signal.Value);
            Assert.IsTrue(signal.HasValue);

            // Does NOT send - value is the same as cached
            result = signal.SendIfChanged(111, null);
            Assert.IsFalse(result);
            Assert.AreEqual(1, count);
            Assert.AreEqual(System.ValueTuple.Create<int, SimpleClass>(111, null), signal.Value);
            Assert.IsTrue(signal.HasValue);

            // Does send - value is different from cached
            result = signal.SendIfChanged(222, null);
            Assert.IsTrue(result);
            Assert.AreEqual(2, count);
            Assert.AreEqual(System.ValueTuple.Create<int, SimpleClass>(222, null), signal.Value);
            Assert.IsTrue(signal.HasValue);

            var a = new SimpleClass(-77);
            var b = new SimpleClass(-88);
            result = signal.SendIfChanged(222, a);
            Assert.IsTrue(result);
            Assert.AreEqual(3, count);
            Assert.AreEqual(System.ValueTuple.Create<int, SimpleClass>(222, a), signal.Value);
            Assert.IsTrue(signal.HasValue);

            result = signal.SendIfChanged(222, b);
            Assert.IsTrue(result);
            Assert.AreEqual(4, count);
            Assert.AreEqual(System.ValueTuple.Create<int, SimpleClass>(222, b), signal.Value);
            Assert.IsTrue(signal.HasValue);

            result = signal.SendIfChanged(222, null);
            Assert.IsTrue(result);
            Assert.AreEqual(5, count);
            Assert.AreEqual(System.ValueTuple.Create<int, SimpleClass>(222, null), signal.Value);
            Assert.IsTrue(signal.HasValue);
        }
    }

    class UnsubscribeInCallback {
        SubscriptionReceipt _receipt;
        public bool Called { get; private set; } = false;
        public UnsubscribeInCallback(Signal signal) {
            _receipt = signal.Subscribe(HandleSignalThenUnsubscribe);
        }
        void HandleSignalThenUnsubscribe() {
            Called = true;
            _receipt.Unsubscribe();
        }
    }

    class UnsubscribeInCallback<T> {
        SubscriptionReceipt _receipt;
        public T Value { get; private set; } = default(T);
        public UnsubscribeInCallback(Signal<T> signal) {
            _receipt = signal.Subscribe(HandleSignalThenUnsubscribe);
        }
        void HandleSignalThenUnsubscribe(T value) {
            Value = value;
            _receipt.Unsubscribe();
        }
    }

}
