﻿// Copyright © 2016 Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitSharp.Fit.Operators;
using fitSharp.Fit.Runner;
using fitSharp.Fit.Service;
using fitSharp.IO;
using fitSharp.Machine.Application;
using fitSharp.Machine.Engine;
using fitSharp.Samples;
using NUnit.Framework;

namespace fitSharp.Test.NUnit.Fit {
    [TestFixture] public class SuiteRunnerTest {

        Memory memory;
        FolderTestModel folders;

        [SetUp] public void SetUp() {
            memory = new TypeDictionary();
            memory.GetItem<Settings>().InputFolder = "in";
            memory.GetItem<Settings>().OutputFolder = "out";

            folders = new FolderTestModel();
        }

        [Test] public void ExecutesSuiteTearDownLast() {
            AddTestFile(@"in\suiteteardown.html");
            AddTestFile(@"in\zzzz.html");

            RunSuite();

            int tearDown = folders.GetPageContent(@"out\reportIndex.html").IndexOf("suiteteardown.html", StringComparison.Ordinal);
            int otherFile = folders.GetPageContent(@"out\reportIndex.html").IndexOf("zzzz.html", StringComparison.Ordinal);
            Assert.IsTrue(otherFile < tearDown);
        }

        [Test] public void SuiteSetupAndTearDownAreIncludedInDryRun() {
            memory.GetItem<Settings>().DryRun = true;

            AddTestFile(@"in\suitesetup.html");
            AddTestFile(@"in\test.html");
            AddTestFile(@"in\suiteteardown.html");

            var reporter = new CollectingReporter();
            RunSuite(reporter);

            string expected = @"in\suitesetup.html" + Environment.NewLine +
                              @"in\test.html" + Environment.NewLine +
                              @"in\suiteteardown.html" + Environment.NewLine;
            Assert.AreEqual(expected, reporter.Output);
        }

        [Test] public void SetupAndTearDownAreNotIncludedInDryRun() {
            memory.GetItem<Settings>().DryRun = true;

            AddTestFile(@"in\setup.html");
            AddTestFile(@"in\test.html");
            AddTestFile(@"in\teardown.html");

            var reporter = new CollectingReporter();
            RunSuite(reporter);

            string expected = @"in\test.html" + Environment.NewLine;
                              
            Assert.AreEqual(expected, reporter.Output);
        }

        [Test] public void DryRunListsAllApplicableTestFiles() {
            memory.GetItem<Settings>().DryRun = true;

            AddTestFile(@"in\test1.html");
            AddTestFile(@"in\test2.html");

            var reporter = new CollectingReporter();
            RunSuite(reporter);

            string expected = @"in\test1.html" + Environment.NewLine +
                              @"in\test2.html" + Environment.NewLine;
            Assert.AreEqual(expected, reporter.Output);
        }

        [Test] public void DryRunDoesNotProduceAnyTestResults() {
            memory.GetItem<Settings>().DryRun = true;

            AddTestFile(@"in\test1.html");
            AddTestFile(@"in\test2.html");

            RunSuite();

            Assert.IsEmpty(folders.GetFiles("out"));
            Assert.IsEmpty(folders.GetFolders("out"));
        }

        [Test]
        public void TestPagePathIsStoredInMemoryWhenPageExecutes() {
            Assert.IsNull(memory.GetItem<Context>().TestPagePath);

            AddTestFile(@"in\page.html");
            RunSuite();

            Assert.AreEqual(@"in\page.html", memory.GetItem<Context>().TestPagePath.ToString());
        }

        [Test]
        public void SetupAndTearDownAreNotCopiedToOutputDirectory() {
            AddTestFile(@"in\setup.html");
            AddTestFile(@"in\test.html");
            AddTestFile(@"in\teardown.html");

            RunSuite();

            Assert.IsTrue(folders.FileExists(@"out\test.html"), "test.html should exist in output directory");

            Assert.IsFalse(folders.FileExists(@"out\setup.html"), "setup.html should not exist in output directory");
            Assert.IsFalse(folders.FileExists(@"out\teardown.html"), "teardown.html should not exist in output directory");
        }

        [Test]
        public void StylesheetIsCreatedInOutputDirectory() {
            AddTestFile(@"in\test.html");

            RunSuite();

            Assert.IsTrue(folders.FileExists(@"out\fit.css"), "fit.css should exist in output directory");
        }

        private void RunSuite() {
            RunSuite(new NullReporter());
        }

        private void RunSuite(ProgressReporter reporter) {
            var runner = new SuiteRunner(memory, reporter, m => new CellProcessorBase(m, m.GetItem<CellOperators>()));
            runner.Run(new StoryTestFolder(memory, folders), string.Empty);
        }

        private void AddTestFile(string path) {
            folders.MakeFile(path, "<table><tr><td>fixture</td></tr></table>");
        }
    }
}
