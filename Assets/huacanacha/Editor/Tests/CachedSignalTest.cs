﻿namespace Tests
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
            signal.Subscribe(dontCallMe);
            signal.Subscribe(callMe);
            Assert.IsFalse(signal.HasValue);
            Assert.IsFalse(calledMe);

            // After firing signal
            signal.UnsubscribeByCallback(dontCallMe);
            signal.Send();
            Assert.IsTrue(signal.HasValue);
            Assert.IsTrue(calledMe);

            // After removing listener & firing
            calledMe = false;
            signal.UnsubscribeByCallback(callMe);
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


    }
}
