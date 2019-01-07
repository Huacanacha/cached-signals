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
            signal.AddListener(dontCallMe);
            signal.AddListener(callMe);
            Assert.IsFalse(signal.HasDispatched);
            Assert.IsFalse(calledMe);

            // After firing signal
            signal.RemoveListener(dontCallMe);
            signal.Dispatch();
            Assert.IsTrue(signal.HasDispatched);
            Assert.IsTrue(calledMe);

            // After removing listener & firing
            calledMe = false;
            signal.RemoveListener(callMe);
            signal.Dispatch();
            Assert.IsFalse(calledMe);

            // Test that cached signal is fired on AddListener
            bool calledMeNow = false;
            System.Action callMeNow = () => {calledMeNow = true;};
            signal.AddListener(callMeNow);
            Assert.IsTrue(calledMeNow);

            // After clearing fired signal
            signal.ClearCache();
            Assert.IsFalse(signal.HasDispatched);
        }

        [Test]
        public void ZeroParameterAddOnceTest() {
            // *** Case A - add then dispatch ***
            var signal = new CachedSignal();
            bool aaa = false;
            System.Action callbackA = () => {aaa = true;};

            // Before firing signal
            signal.AddOnce(callbackA);
            Assert.IsFalse(aaa);
            Assert.IsFalse(signal.HasDispatched);

            // After firing signal
            signal.Dispatch();
            Assert.IsTrue(aaa);

            // After second firing - listener should be removed
            aaa = false;
            signal.Dispatch();
            Assert.IsFalse(aaa);

            // *** Case B - add listener after signal has been dispatched ***
            signal = new CachedSignal();
            bool bbb = false;
            System.Action callbackB = () => {bbb = true;};

            Assert.IsFalse(bbb);
            signal.Dispatch();
            signal.AddOnce(callbackB);
            Assert.IsTrue(bbb);

            // Now check that the listener wasn't added
            bbb = false;
            Assert.IsFalse(bbb);
            signal.Dispatch();
            Assert.IsFalse(bbb);

            // *** Case C - multiple listeners ***
            signal = new CachedSignal();
            aaa = false;
            bbb = false;

            signal.AddOnce(callbackA);
            signal.AddOnce(callbackB);
            Assert.IsFalse(aaa);
            Assert.IsFalse(bbb);

            signal.Dispatch();

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
            signal.AddListener(setMe);
            signal.AddListener(dontCallMe);
            Assert.IsFalse(signal.HasDispatched);
            Assert.IsFalse(calledMe);

            // After firing signal
            signal.RemoveListener(dontCallMe);
            signal.Dispatch(1);
            Assert.IsTrue(signal.HasDispatched);
            Assert.IsTrue(calledMe);
            Assert.AreEqual(1, setMeInt);

            // After firing signal again
            signal.Dispatch(42);
            Assert.AreEqual(42, setMeInt);

            // After removing listener & firing
            calledMe = false;
            signal.RemoveListener(setMe);
            signal.Dispatch(77);
            Assert.IsFalse(calledMe);
            Assert.AreNotEqual(77, setMeInt);

            // Test that cached signal is fired on AddListener
            signal.Dispatch(144);
            int setMeNowInt = -1;
            System.Action<int> callMeNow = (a) => {setMeNowInt = a;};
            signal.AddListener(callMeNow);
            Assert.AreEqual(144, setMeNowInt);

            // After clearing fired signal
            signal.ClearCache();
            Assert.IsFalse(signal.HasDispatched);
        }

        [Test]
        public void OneParameterAddOnceTest() {
            // *** Case A - add then dispatch ***
            var signal = new CachedSignal<int>();
            int aaa = -1;
            System.Action<int> callbackA = (value) => {aaa = value;};

            // Before firing signal
            signal.AddOnce(callbackA);
            Assert.AreEqual(-1, aaa);
            Assert.IsFalse(signal.HasDispatched);

            // After firing signal
            signal.Dispatch(3);
            Assert.AreEqual(3, aaa);

            // After second firing - listener should be removed
            aaa = -1;
            signal.Dispatch(5);
            Assert.AreEqual(-1, aaa);

            // *** Case B - add listener after signal has been dispatched ***
            signal = new CachedSignal<int>();
            int bbb = -1;
            System.Action<int> callbackB = (value) => {bbb = value;};

            Assert.AreEqual(-1, bbb);
            signal.Dispatch(7);
            signal.AddOnce(callbackB);
            Assert.AreEqual(7, bbb);

            // Now check that the listener wasn't added to fire again
            bbb = -1;
            Assert.AreEqual(-1, bbb);
            signal.Dispatch(9);
            Assert.AreEqual(-1, bbb);

            // *** Case C - multiple listeners ***
            signal = new CachedSignal<int>();
            aaa = -1;
            bbb = -1;

            signal.AddOnce(callbackA);
            signal.AddOnce(callbackB);
            Assert.AreEqual(-1, aaa);
            Assert.AreEqual(-1, bbb);

            signal.Dispatch(42);
            Assert.AreEqual(42, aaa);
            Assert.AreEqual(42, bbb);

            // *** Case C - AddOnce after clearing ***
            signal = new CachedSignal<int>();
            aaa = -1;
            bbb = -1;

            signal.AddOnce(callbackA);
            Assert.AreEqual(-1, aaa);
            Assert.AreEqual(-1, bbb);

            signal.Dispatch(66);
            Assert.AreEqual(66, aaa);
            Assert.AreEqual(-1, bbb);

            signal.ClearCache();
            signal.AddOnce(callbackB);
            Assert.AreEqual(66, aaa);
            Assert.AreEqual(-1, bbb);

            signal.Dispatch(77);
            Assert.AreEqual(66, aaa);
            Assert.AreEqual(77, bbb);
        }

    }
}
