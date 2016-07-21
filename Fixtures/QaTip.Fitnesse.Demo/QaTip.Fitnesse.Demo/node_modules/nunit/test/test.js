/**
 * Using the NUnit.js to test NUnit.js
 */


(function aFunction(global, nunit, require){
	// var nunit = (function(){
	// 	var nunit;
	// 	if(global === global.window || typeof window !== "undefined" && window.window === window) {
	// 		//In browser
	// 		nunit = window.NUnit? window.NUnit: (typeof require !== "undefined"? require("nunit"): undefined);

	// 	}else if(typeof module !== "undefined" && global === module.exports){
	// 		nunit = require("nunit");
	// 	}
	// 	if(!nunit) throw Error("This test requires NUnit.js");
	// 	return nunit ;
	// })();

	nunit = nunit? nunit: (require? require("nunit"): undefined);
	if(!nunit) throw Error("This test requires NUnit.js");

	var test = new nunit.Test("Using NUnit.js to test NUnit.js test"); 
	var assert = test.assert ;
	test.testConstructor = function(){
		var t = new nunit.Test();
		assert.isTrue(t instanceof nunit.Test);
		t = nunit.Test();
		assert.isTrue(t instanceof nunit.Test);
	};

	test.testName = function(){
		var t = new nunit.Test('mytest');
		assert.eq("mytest", t.desc);
	};

	/** @memberOf test */
	test.testEq = function(){
		assert.eq("", "");
		
		//When not equal, there will be an exception and err should not be null
		var err;
		try{
			assert.eq("", 123, "When not equal, there will be an exception and err should not be null");
		}catch(e){err = e;}
		assert.notNull(err);
		//assertEquals("When not equal, there will be an exception and err should not be null", err.message);

	};

	test.testNotEq = function(){
		assert.notEquals("", "1");
		assert.notEqual(1, "1");
		assert.exception(function(){
			assert.notEqual(0, 0);
		}, "");
	};

	test.assertTrue = function(){
		assert.isTrue(true);
		assert.isFalse(false);
		var err;
		try{
			assert.isTrue({}, "desc");
		}catch(e){err = e;}
		assert.notNull(err);
		assert.eq("desc", err.message);
		
		var err;
		try{
			assert.isTrue({});
		}catch(e){err = e;}
		assert.notNull(err);
		assert.eq("Expecting true", err.message);
		
		
		var err;
		try{
			assert.isFalse("");
		}catch(e){err = e;}
		assert.notNull(err);
		assert.eq("Expecting false", err.message);
		
		var err;
		try{
			assert.isFalse(1, "desc");
		}catch(e){err = e;}
		assert.notNull(err);
		assert.eq("desc", err.message);
		
	}

	test.isNull = function(){
		assert.isNull(null);
		assert.isNull(undefined);
		
		assert.notNull("");
		assert.notNull(1);
		assert.notNull(0);
		
		
		var err;
		try{
			assert.isNull("null", "should be null");
		}catch(e){err = e;}
		assert.notNull(err);
		assert.eq("should be null", err.message);
		
		var err;
		try{
			assert.isNull(1);
		}catch(e){err = e;}
		assert.notNull(err);
		assert.eq("Expecting null", err.message);
		
	};

	test.testFail = function(){
		var err;
		try{
			assert.fail();
		}catch(e){err = e;}
		assert.notNull(err);
		assert.eq("", err.message);
		
		var err;
		try{
			assert.fail("msg");
		}catch(e){err = e;}
		assert.notNull(err);
		assert.eq("msg", err.message);
		
	};

	test.testTestClass = function(){
		var a = assert;
		var t = new nunit.Test();
		a.isTrue(t instanceof nunit.Test);
		t.testOne = function(){};
	//	a.equals(1, Object.getOwnPropertyNames(t).length);

	};

	test.testRunner = function(){
		var a = assert;
		
		var runner = new nunit.TestRunner();

		var err;
		try{
			runner.run();
		}catch(e){
			err = e;
		}
		a.notNull(err, "No test to run will cause error");

		var t = new nunit.Test();
		t.myTest = function(){ a.fail();};
		runner.run(t);

	};

	test.testeReporter = function(){
		var a = assert ;
		var tr = a.tracer();
		var count  = 0 ;
		var t = new nunit.Test();
		t.test1 = function(){};
		t.test2 = function(){ a.fail()};
		var reporter =  {
			/** @param {Number} testCount */
			/** @param {String} desc */
			testUnitBegin: function(testCount, desc){
				tr.once("testUnitBegin");
				a.equals(2, testCount);
				a.equals(t.desc, desc);
			},
			/** @param {String} testName  */
			testBegin: function(id, testName){
				tr.once("testBegin");
				a.isTrue(id === t.id + ".test1" || id === t.id + ".test2");
				a.isTrue(testName === "test1" || testName === "test2");
			},
			/** @param {NUnit.Test} test  */
			/** @param {String} testName  */
			/** @param {Boolean} result  */
			testEnd: function(id, testName, result){
				tr.once("testEnd");
				a.isTrue(id === t.id + ".test1" || id === t.id + ".test2");
				a.isTrue(testName === "test1" || testName === "test2");
				a.isTrue (result.passed === true || result.passed === false);
			},

			testUnitEnd: function(testCount, desc, failedCount){
				tr.once("testUnitEnd");
				a.equals(2, testCount);
				a.equals(t.desc, desc);
				a.eq(1, failedCount);
			}

		};

		var runner = new nunit.TestRunner();
		runner.addReporter(reporter);
		//runner.debug = false;
		runner.run(t);
		tr.verify(4, "4 method should all be executed at lease once");
	};

	test.testContains = function(){
		assert.contains("abc", "c");

	}

	test.testConfig = function(){
		var a = assert ;
		var opt = nunit.defaultConfig();
		a.notNull(opt.debug);
		a.notNull(opt.tests);
		a.notNull(opt.reporters);
		a.eq("ConsoleReporter", opt.reporters[0].id);

		//saves global config
		var cfg = nunit.config();

		nunit.config({reporters:[{id: "any"}],
			tests:[new Object], debug: true});

		var opt = nunit.config();

		a.notNull(opt.debug);
		a.t(opt.debug)
		a.notNull(opt.tests);
		a.notNull(opt.reporters);
		a.eq(1, opt.tests.length);
		a.eq(1, opt.reporters.length);
		a.eq("any", opt.reporters[0].id);

		//reset global config
		nunit.config(cfg);
	}

	test.testBeforeAfter = function(a){
		var t = new nunit.Test();
		var count = 0;
		var count2 = 0;
		var count3 = 0;
		var count4 = 0;
		t.beforeAll = function(){
			count ++ ;
		};

		t.afterAll = function(){
			count2 ++ ;
		};

		t.before = function(){
			count3 ++ ;
		};

		t.after = function(){
			count4 ++ ;
		};

		t.test1 = function(){};
		t.test2 = t.test1 ;
		t.test3 = t.test1 ;

		var runner = new nunit.TestRunner();
		runner.run(t);
		a.eq(1, count);
		a.eq(1, count2);
		a.eq(3, count3);
		a.eq(3, count4);

		
	};

	test.isTestCase = function(a){
		var Test = nunit.Test ;
		a.f(Test.isValidTestName("before"));
		a.f(Test.isValidTestName("after"));
		a.f(Test.isValidTestName("beforeAll"));
		a.f(Test.isValidTestName("afterAll"));
		a.f(Test.isValidTestName(null));
		a.f(Test.isValidTestName(undefined));
		a.f(Test.isValidTestName(""));

		a.t(Test.isValidTestName("beforeEach"));
		a.t(Test.isValidTestName("afterEach"));
		a.t(Test.isValidTestName("any"));
		a.t(Test.isValidTestName("goodName"));
	};

	test.testAssertCount = function(){
		var t = new nunit.Test();
		t.m1 = function(a){
			a.eq(1, 1);
			a.t(true);
		};
		t.m2 = function(a){
			a.strictEquals(1,1)
		};

		nunit.TestRunner().run(t);
		assert.eq(3, t.assert.assertCount, "Three asserts in two test cases.");
		assert.eq(2, t.results.m1.assertCount);
		assert.eq(1, t.results.m2.assertCount);
		assert.notEqual(0, "0");
	}

	test.testTracer = function(a){
		var tr = a.tracer();
		a.notNull(tr);
		for(var i = 0 ; i < 10; i++){
			tr.trace();
			tr.once();
			tr.once();//same id as tr.once() above, so it will take no effect
			tr.once("second trace.once()");
		}

		tr.verify(12);
	}

	test.testAliases = function(a){
		/*
	makeAlias(assert, "strictEquals", ["strictEqual", "assertStrictEqual", "assertStrictEquals"]);
	makeAlias(assert, "equals", ["eq", "equal", "assertEquals", "assertEqual"]);
	makeAlias(assert, "notEqual", ["neq", "notEquals", "assertNotEqual"]);
	makeAlias(assert, "isTrue", ["t", "assertTrue"]);
	makeAlias(assert, "isFalse", ["f", "assertFalse"]);
	makeAlias(assert, "isNull", ["assertNull"]);
	makeAlias(assert, "notNull", ["assertNotNull"]);
	makeAlias(assert, "contains", ["contain"]);
		*/
		a.strictEquals(a.strictEquals, a.strictEqual);
		a.strictEquals(a.strictEquals, a.assertStrictEqual);
		a.strictEquals(a.strictEquals, a.assertStrictEquals);

		a.strictEquals(a.equals, a.eq);
		a.strictEquals(a.equals, a.equal);
		a.strictEquals(a.equals, a.assertEquals);
		a.strictEquals(a.equals, a.assertEqual);

	}


	test.doFail = function(){
		//assert.fail("This should fail.");
	}

	typeof module !== "undefined" ? module.exports = test : "";
})(this, (typeof NUnit !== "undefined"? NUnit: undefined), typeof require !== "undefined"? require: "");

