// Copyright � 2012 Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using NUnit.Framework;
using fitSharp.Fit.Operators;

namespace fitSharp.Test.NUnit.Fit 

{
    [TestFixture] public class ParseNullTest : ParseOperatorTest<ParseNull> {
        [Test] public void CanParse() {
            Assert.IsTrue(CanParse<string>("null"), "null");
            Assert.IsTrue(CanParse<string>("NULL"), "NULL");
            Assert.IsTrue(CanParse<string>("\r\n null \r\n\t"), "null with whitespace");
            Assert.IsFalse(CanParse<string>("not null"), "not null");
        }

        [Test] public void ParseAlwaysReturnsNull() {
            Assert.IsNull(Parse<string>("null"), "null");
            Assert.IsNull(Parse<string>("not null"), "not null");
        }
    }
}
