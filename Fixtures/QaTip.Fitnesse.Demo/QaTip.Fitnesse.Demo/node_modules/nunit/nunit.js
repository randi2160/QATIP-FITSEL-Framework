/**
 *  Copyright (c) 2013 Neal Xiong
 */

(function(global){
	
	var NUnit = {};
	
	NUnit.Assert = function(test){
		this.test = test ;
		this.assertCount = 0;
	};

	var assert = {
		"strictEquals": function(obj1, obj2, desc){
			if(obj1 === obj2) return true ;
			throw Error("Expecting " + toStr(obj1) + " but was " + toStr(obj2));
		},
		/** 
		 * Assert equals. Do not use to compare two null value. Use {@link #isNull} to assert a null(or undefined) value.
		 * @memberOf assert 
		 * */
		"equals": function(obj1, obj2, desc){
			if(obj1 === obj2 || toStr(obj1) == toStr(obj2)){
				return true ;
			};
			throw new Error(desc || "Expecting " + toStr(obj1) + " but was " + toStr(obj2)) ;
		},

		"notEquals": function(obj1, obj2, desc){
			var _assert = this ;
			return this.exception(function(){
				_assert.equals(obj1, obj2);
			}, "Expecting " + toStr(obj1) + " to be not equal to " + toStr(obj2));
		},
		// "notEqual": function(obj1, obj2, desc){
		// 	return this.notEquals(obj1, obj2, desc);
		// },
		
		"isTrue": function(obj, desc){
			if(obj === true){
				return true ;
			}
			throw new Error(desc || "Expecting true");
		},
		"isFalse": function(obj, desc){
			if(obj === false){
				return true ;
			}
			throw new Error(desc || "Expecting false");
		},
		"isNull": function(obj, desc){
			if(isNull(obj)){
				return true ;
			}
			throw Error(desc || "Expecting null");
		},
		"notNull": function(obj, desc){
			if(!isNull(obj)){
				return true ;
			}
			throw Error(desc || "Expecting not null");
		},
		"fail":  function(msg){
			msg = isNull(msg)? "": msg ;
			throw Error(msg);
		},
		"contains": function(obj1, obj2, desc){
			try{
				return this.isTrue(obj1.indexOf(obj2) > -1, desc)
			}catch(e){
				throw Error("Expecting " + toStr(obj2) + " in " + toStr(obj1));
			}
		},
		"exception": function(callback, desc){
			var err ;
			try{
				//FIXME ?? context of callback?
				callback();
			}catch(e){
				err = e ;
			}
			return this.notNull(err, desc || "Expecting exception from closure.");
		}
		// "guarantee": function(){
		// 	return {
		// 		counter: 0,
		// 		crossPoints: [],
		// 		cross: function(desc){
		// 			this.crossPoints.push(desc);
		// 			this.counter ++;
		// 		},
		// 		count : function(num, desc){
		// 			try{
		// 				var pass = assert.eq(this.counter, num, desc);	
		// 				this.counter = 0;
		// 				this.crossPoints = [];
		// 			}catch(e){
		// 				var msg = desc || e.message ;
		// 				throw Error(msg + "\n" + "Captured cross points: " + toStr(this.crossPoints) );
		// 			}
					
		// 		}

		// 	};
		// }
		
	};
	//
	var makeAlias = function(targetObj, targetFuncName, aliases ){
		if(!targetObj || !targetObj[targetFuncName] || !aliases){
			return ;
		}

		if(typeof aliases == "string"){
			if(typeof targetObj[aliases] == "undefined"){
				targetObj[aliases] = targetObj[targetFuncName] ;
			}else{
				//? already exists
			}
		}else if(aliases.length){
			for(var i = 0 ;i < aliases.length; i ++){
				makeAlias(targetObj, targetFuncName, aliases[i]);
			}
		}

	};

	// makeAlias(assert, "strictEquals", ["strictEqual", "assertStrictEqual", "assertStrictEquals"]);
	// makeAlias(assert, "equals", ["eq", "equal", "assertEquals", "assertEqual"]);
	// makeAlias(assert, "notEquals", ["neq", "notEqual", "assertNotEqual", "assertNotEquals"]);
	// makeAlias(assert, "isTrue", ["t", "assertTrue"]);
	// makeAlias(assert, "isFalse", ["f", "assertFalse"]);
	// makeAlias(assert, "isNull", ["assertNull"]);
	// makeAlias(assert, "notNull", ["assertNotNull"]);
	// makeAlias(assert, "contains", ["contain"]);

	/** Dispatch the function call to assert.prop(params) */
	NUnit.Assert._dispatch = function(a, prop, params){
		try{
			var ret = invoke(assert, prop, params);
			a.assertCount ++ ;
			return ret ;
		}catch(e){
			for(var idx in params){
				params[idx] = toStr(params[idx]);
			}
			e.assertStack = e.assertStack || [];
			e.assertStack.push("at " + prop + "(" + params.join(",") + ")");
			throw e;
		} 
	};


	// Dispatch all the function call from NUnit.Asssert.prototype.prop to assert.prop
	for(var prop in assert){
		if(typeof assert[prop] == "function"){
			NUnit.Assert.prototype[prop] = (function(prop){
				return function(){
					return NUnit.Assert._dispatch(this, prop, argsToArray(arguments));
				};
			})(prop);
		}
	};

	makeAlias(NUnit.Assert.prototype, "strictEquals", ["strictEqual", "assertStrictEqual", "assertStrictEquals"]);
	makeAlias(NUnit.Assert.prototype, "equals", ["eq", "equal", "assertEquals", "assertEqual"]);
	makeAlias(NUnit.Assert.prototype, "notEquals", ["neq", "notEqual", "assertNotEqual", "assertNotEquals"]);
	makeAlias(NUnit.Assert.prototype, "isTrue", ["t", "assertTrue"]);
	makeAlias(NUnit.Assert.prototype, "isFalse", ["f", "assertFalse"]);
	makeAlias(NUnit.Assert.prototype, "isNull", ["assertNull"]);
	makeAlias(NUnit.Assert.prototype, "notNull", ["assertNotNull"]);
	makeAlias(NUnit.Assert.prototype, "contains", ["contain"]);

	NUnit.Assert.prototype.tracer = function(desc){
		return new Tracer(this);
	}

	var Tracer = function(assert){
		this.assert = assert ;
		this.traceQueue = [];
		this.traceCount = 0;
		this.traceMap = {};

	};
	Tracer.prototype = {
		trace: function(desc){
			this.traceCount ++ ;
			this.traceQueue.push(["trace", desc]);
		},
		once: function(desc){
			var key = toStr(desc);
			var val = this.traceMap[key];
			if(val){
				this.traceMap[key] = val + 1;
			}else{
				this.traceMap[key] = 1;
				this.traceQueue.push(["traceOnce", desc]);
			}
		},
		verify: function(count, desc){
			try{
				switch(typeof count){
				case "number" :
						return this.assert.eq(count, this.traceQueue.length, desc);
						break;
					case "object":
						default:

				}	
			}catch(e){
				//Failed verification
				if(e.assertStack){
					e.assertStack.push("at tracer.verify(" + toStr(count) + ", " + toStr(desc) + ")");
				}
				throw e ;
			}
					
		}
	};


	/** 
	 *  Uses JSON.stringify to convert the object into a string.
	 *  @private */
	var toStr = function(obj){
		if(typeof obj == "function"){
			return obj.toString();
		}
		if (isNull(obj)){
			return "" + obj;
		}
		
		return JSON.stringify(obj);
	};

	/** 
	 * Returns true if obj is null of undefined 
	 * @private
	 * */
	var isNull = function(obj){
		/* null === undefined is false 
		 * null == undefined is true */
		return obj == undefined ;
	};
	/** Return an array containing all the parameters in the argument object (used to delegate function calls). 
	 @private 
	 @params {Arguments}  args */
	var argsToArray = function(args){
		var arr = [];
		if(args && args.length)
		for(var i = 0 ;i < args.length; i ++){
			arr.push(args[i]);
		}
		return arr ;
	};
	var invoke = function(thisArg, funcName, params){
		var alen = arguments.length ;
		if(alen < 2 || !thisArg){
			throw Error("Must have thisArg and funcName");
		}
		var cParams = [];
		var func = thisArg[funcName] ;
		switch(alen){
			case 2:
				break;
			case 3:
				Array.isArray(params) ? cParams = params: cParams = [params] ;
				break;
			default:
				// > 3
				for(var i = 2 ; i < alen ; i ++){
					cParams.push(arguments[i]);
				}
		}
	//	cParams.unshift(thisArg);
		if(func && typeof func == "function"){
	//		return func.call.apply(func, cParams) ;
			return func.apply(thisArg, cParams) ;
		}else{
			throw Error( funcName + " is not found on object.");
		}
	};
	
	var Test = NUnit.Test = function(desc){
		if(this instanceof Test == false){
			return new Test(desc);
		}
		this.id = "_" + Date.now();
		this.desc = desc || "";
		this.assert = new NUnit.Assert(this);
		/** 
		 * A map of result objects - function name -> result
		 * @field */
		this.results = {};

		//Add new test object into Test.instances array
		Test.instances.push(this);
	};


	var EMPTY_FUNC = function(){};
	Test.prototype.before = EMPTY_FUNC ;
	Test.prototype.after = EMPTY_FUNC ;
	Test.prototype.beforeEach = EMPTY_FUNC ;
	Test.prototype.afterEach = EMPTY_FUNC ;
	Test.prototype.hasTest = function(funcName){
		return this.hasOwnProperty(funcName) && typeof this[funcName] === "function" && Test.isValidTestName(funcName);
	};
	Test.beforeClass = EMPTY_FUNC ;
	Test.afterClass = EMPTY_FUNC ;
	Test.BEFORE_ALL  = "beforeAll" ;
	Test.AFTER_ALL = "afterAll" ;
	Test.BEFORE  = "before" ;
	Test.AFTER = "after" ;
	Test.BEFORE_CLASS = "beforeClass" ;
	Test.AFTER_CLASS = "afterClass";

	/**Keep track of all instances of Test */
	Test.instances = [];

	/** Returns true if the argument is a valid test case function name */
	Test.isValidTestName = function(funcName){
		if(!funcName || funcName === Test.BEFORE_ALL || funcName === Test.AFTER_ALL || funcName === Test.BEFORE || funcName === Test.AFTER){
			return false ;
		}
		return true ;
	};
	
	var TestRunner = NUnit.TestRunner = function(){
		if(this instanceof TestRunner == false){
			return new TestRunner();
		}
		/** 
		 * A list of result objects
		 * @field */
		this.results = [];
		this.total = 0;
		this.failed = [];
		/**
		 * A list of appenders
		 */
		this.appenders = [];
		this.reporters = [];
	};
	TestRunner.prototype = {
			/** @param  {Test} test */
			run: function(test){
				var total = 0, failed = 0 , successful = 0, target = test ;
//				this.out.println('log', '[TestRunner] ' + 'Starting test.');
				if(!test || !test instanceof Test ){
					throw new Error('No test found.');
					return ;
				}

				// Count number of total tests
				for(var attr in test){
					if(test.hasTest(attr)){
						total ++;
					}
				}
				// failedTarget={};

				this.info('[TestRunner] ' + 'Starting test.');
				this.report("testUnitBegin", [total, test.desc]);

				
				//Run Test.before
				if(test.hasOwnProperty(Test.BEFORE_ALL) ){
					this.log('[TestRunner] ' + 'Running BEFORE_ALL function.');
					invoke(test, Test.BEFORE_ALL);
				}
				for(var prop in test){
					if(test.hasTest(prop)){
						
						if(test.hasOwnProperty(Test.BEFORE) && typeof test[Test.BEFORE] === "function"){
							this.log('[TestRunner] ' + 'Running BEFORE.');
							try{
								invoke(test,Test.BEFORE);
							}catch(e){
								;
							}
							
						}
						
						if(typeof test[prop] === "function"){
							this.log('[TestRunner] ' + 'Running #' + prop);
							var result = new TestResult(test, prop);
							this.report("testBegin", [result.id, prop]);
							this.results.push(result);
							test.results[prop] = result ;
							// total ++ ;
							var preCount = test.assert.assertCount ;
							try{
								result.startTime = Date.now();
								invoke(test,prop, test.assert) ;
								result.endTime = Date.now();
								successful ++ ;
								result.passed = true;//result.passed = false by default
								result.assertCount = test.assert.assertCount  - preCount ;
							}catch(e){
								failed ++ ;
								result.endTime = Date.now();
								result.assertCount = test.assert.assertCount  - preCount ;
								
								var msg = e.message;
								if(e.assertStack){
									e.assertStack.push(result.assertCount + " asserts so far.");
									msg += " " + e.assertStack.join("\n") ;
								}
								this.err("[#" + prop +"] " + msg);
								this.report("println", ["ERROR", "[#" + prop +"] " + msg]);
								//this.err("Failed: #"+ prop );
								//this.report("println", ["ERROR", "Failed: #"+ prop ]);
								result.error = e;
							}finally{
								this.report("testEnd", [result.id, prop, result]);
							}
							//Put result into
						}

						if(test.hasOwnProperty(Test.AFTER) && typeof test[Test.AFTER] === "function"){
							this.log('[TestRunner] ' + 'Running AFTER');
							invoke(test,Test.AFTER);
						}
					}
				}
				if(test.hasOwnProperty(Test.AFTER_ALL) ){
					this.log('[TestRunner] ' + 'Running AFTERALL');
					invoke(test, Test.AFTER_ALL);
				}
				this.report("testUnitEnd", [total, test.desc, failed]);
				this.log('[TestRunner] ' + 'Finishing test.');
				this.info('[TestRunner] ' + 'Ran ' + total + ' tests, ' + successful + ' were successful, ' + failed + ' were failed.');
			},
			info: function(msg){
				this._log("INFO", msg)
			},
			error: function(msg){
				this._log("ERROR", msg);
			},
			err: function(msg){
				this.error(msg);
			},
			log: function(msg){
				this._log("LOG", msg);
			},
			_log: function(level, msg, skipAppenders){
				var debug = this.debug ;
				if(console && debug){
					switch(level){
						case "LOG":
							console.log(msg);
							break;
						case "INFO":
							console.info(msg);
							break;
						case "ERROR":
							console.error(msg);
							break ;
					}
				}
				if(this.appenders && !skipAppenders){
					for(var index in this.appenders){
						try{
							this.appenders[index]['append'](level, msg);
						}catch(e){
							this._log("ERROR", e.message, true);
						}
						
					}
					
				}
			},
			addReporter: function(reporter){
				if(typeof reporter == "string") {
					switch(reporter){
						case "ConsoleReporter":
							reporter = ConsoleReporter;
							break;
					}
				}
				this.reporters.push(reporter);
			},
			report: function(event, params){
				for(var index in this.reporters){
					try{
						var rep = this.reporters[index] ;
						if(typeof rep[event] == "function") rep[event].apply(rep, params);
					}catch(e){
						if(console){
							console.error("Error in reporter." + event + "(): " + e.message);
						}
						throw e ;
					}
				}
			},
			debug : false 
			
	};

	var TestResult = NUnit.TestResult = function(test, testName){
		this.testName = testName ;
		this.id = test.id + "." + testName;
		this.passed = false ;
		this.error = null;
		this.assertCount = 0;
		this.startTime = Date.now();
		this.endTime = -1 ;
	};

	/** Global config object */
	var _config = null;
	/** Gets global config object */
	var _getConfig = function(){
		_config  = _config  || NUnit.defaultConfig();
		return _config ;
	};
	/** Copy all the attributes that options does not have from optTemplate to options */
	var _replicateConfig = function(options, optTemplate){
		for(var attr in optTemplate){
			if(typeof options[attr] === "undefined"){
				options[attr] = optTemplate[attr];
			}
		}
	};

	/** Returns default config */
	NUnit.defaultConfig = function(){
		return {
				tests: Test.instances,
				reporters: [ConsoleReporter],
				debug: false
			};
	};
	/** Get or set global config */
	NUnit.config = function(options){
		if(options){
			_replicateConfig(options, _getConfig())
			_config = options ;
		}
		return _getConfig();
	};

	/** Convenient method to run all instances of Test */
	NUnit.execute = function(opt){
		opt = opt || {} ;
		_replicateConfig(opt, _getConfig()) ;
		var runner = new NUnit.TestRunner();
		if(opt.debug){
			runner.debug = true ;
		}
		for(var index in opt.reporters){
			runner.addReporter(opt.reporters[index]);
		}
		for(var index in opt.tests){
			runner.run(opt.tests[index]);
		}
	}

	var ConsoleReporter = {
		id: "ConsoleReporter",
		/** @param {Number} testCount */
		/** @param {String} desc */
		testUnitBegin: function(testCount, desc){
			this.println("INFO", 'Starting to run ' + testCount + ' tests in module: ' + desc);
		},
		/** @param {String} testName  */
		testBegin: function(id, testName){
			//this.println("INFO", "Running #" + testName);
		},
		/** @param {NUnit.Test} test  */
		/** @param {String} testName  */
		/** @param {Boolean} result  */
		testEnd: function(id, testName, result){
			if(result.passed){
				this.println("INFO", "PASSED" + " " + "#" + testName + "(). " + result.assertCount +  " asserts.");
			}else{
				this.println("ERROR", "FAILED" + " " + "#" + testName + "(). ");
			}

		},

		testUnitEnd: function(testCount, desc, failedCount){
			this.println("INFO", "Finishing test module. Ran " + testCount + " tests. " + (testCount - failedCount) + " were successful.");
			if(failedCount > 0) this.println("ERROR", failedCount + " tests were failed.");
		},
		println: function(level, msg){
			var id = "[" + this.id + "] " ;
			if(console){
				switch(level){
					case "LOG":
						console.log(id + msg);
						break;
					case "INFO":
						console.info(id + msg);
						break;
					case "ERROR":
						console.error(id + msg);
						break ;
				}
			}
		}
	}					

	if(typeof module !== "undefined") module.exports = NUnit ;
	if(global && global.window === global){
		global.NUnit = NUnit ;
	}

	
})(this);
