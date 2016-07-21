(function(nunit){
	
	var styleText = ""
+ "#nunitReport{"
+ "font-family: Arial, Helvetica, sans-serif;"
+ "min-width: 200px;"
+ "max-width: 200px;"
+ "width: 200px;"
+ "position: fixed;"
+ "right: 3px;"
+ "bottom: 168px;"
+ "/*border:1px solid gray;*/"
+ "}"
+ "#nunitSummary{"
+ "	height: 20px;"
+ "	max-height: 20px;"
+ "	min-height: 20px;"
+ "	border:1px ridge #808080 ;"
+ "	padding: 4px 0 0 4px;"
+ "	/*font-weight: bolder;*/"
+ "	font-size: small;"
+ "}"
+ "#nunitError{"
+ "	position: absolute;"
+ "	bottom: -103px;"
+ "	width: 100%;"
+ "	max-height: 100px;"
+ "	min-height: 100px;"
+ "	border:2px solid #BDBDBD ;"
+ "	border-top: 0;"
+ "	padding: 3px 0 0 3px;"
+ "	resize: vertical;"
+ "	font-family:\"Lucida Console\", Monaco, monospace;"
+ "	font-size: 13px;"
+ "}"
+ ""
+ "#nunitDetail{"
+ "	max-height: 200px;"
+ "	min-height: 20px;"
+ "	overflow-y: scroll;"
+ "	border:1px inset #808080 ;"
+ "	border-top: 0;"
+ "	padding: 3px 0 0 3px;"
+ "	resize: vertical;"
+ "}"
+ ".nunit-error-msg{"
+ "	color: red;"
+ "}"
+ ".nunit-unknown{"
+ "	background-color: rgba(131, 131, 131, 0.75);"
+ "	/*color: white;*/"
+ "}"
+ ""
+ ".nunit-all-pass{"
+ "	background-color: rgba(81, 182, 76, 0.75);"
+ "	/*color: white;*/"
+ "}"
+ ".nunit-any-failed{"
+ "	background-color: rgba(139, 45, 48, 0.75);"
+ "}"
+ ".nunit-single-test{"
+ "	min-height: 18px;"
+ "	font-size: smaller;"
+ "	padding-left: 20px;"
+ "	margin-left: 10px;"
+ "}"
+ ".nunit-single-test.failed{"
+ "	cursor: pointer;"
+ "	text-decoration:underline;"
+ "	background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAMAAAAoLQ9TAAAAwFBMVEXJFiXeHy/obXfocnztjJb4+fzv8/vz9vz5+vzm7frt8vrl7fnp8Prr8frx9fvn7/r2+fzc6+bP49tgrn1irn5lsYFxt4suh005klk9j1pFlWFNn2pRpG9VonBSnG1QmGpbqnhfrHtcpHZZmXBosoNttIdHnWR7dE+fmn+5taJ+dU6HekyDd02Rf0iMfUqdhUWXgkawjz6sjECnikKiiEO7qHbLvpy0kD3///////8AAAAAAAAAAAAAAAAAAAAAAAAFgP5RAAAAOnRSTlP///////////////////////////////////////////////////////////////////////////8AN8D/CgAAAAFiS0dEPz5jMHUAAAAJcEhZcwAAAEgAAABIAEbJaz4AAACOSURBVBjTTc1nE4MgDIBhUJRqKwrdy+491Q74/3+sNNhcn0+59y4JMdb7D4HQQC4UQU8GTgGhzDYyc0oIFW/LLy55BeEp8vmi012LXDwgvMJJWLsRQxxlxSq+2+AxyjyyHU+XyTC52MC0rxlpOVcbqK99CiupSk8YBqPZKupHZ1xpOkc8etjvAL41NRx+PnOgHDBKqXC8AAAAAElFTkSuQmCC);"
+ "	background-repeat: no-repeat;"
+ "}"
+ ".nunit-single-test.passed{"
+ "	background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAMAAAAoLQ9TAAAAwFBMVEX4+fzv8/vz9vz5+vzm7frt8vrl7fnp8Prr8frx9fvn7/r2+fzc6+bP49uMv6XE3tECnktNqHlbsINjuIt+uJmbx7Cpz7u318Zgrn1irn5lsYFxt4suh005klk9j1pFlWFNn2pRpG9VonBSnG1QmGpbqnhfrHtcpHZZmXBosoNttIdHnWR7dE+fmn+5taJ+dU6HekyDd02Rf0iMfUqdhUWXgkawjz6sjECnikKiiEO7qHbLvpy0kD3///////8AAADUu3FjAAAAP3RSTlP//////////////////////////////////////////////////////////////////////////////////wCOJnwXAAAAAWJLR0Q/PmMwdQAAAAlwSFlzAAAASAAAAEgARslrPgAAAJlJREFUGNNdzWcPgjAUheGCoIyCbR24UXHvCTja//+zvPYSYnw+vsnJIQq8fxAdKiUMKelTglIdMn9DfZTpkJsd+mVSM9fh6SbzRbe3dhP3ocPLmBiFG1EMBcAO7DuEBqiz7Xi6dEbOBUMkWQ1dIQghZEtPvMA7QZBRWwo2jGcra2CdITSlDDmroiMEHkrO2WG/08pbVSDqzwfXxCBSUNEG9QAAAABJRU5ErkJggg==);"
+ "	background-repeat: no-repeat;"
+ "}"
+ ".nunit-single-test.test-name{"
+ "	margin-left: 0px;"
+ "	background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAMAAAAoLQ9TAAAAwFBMVEX4+fzv8/vz9vz5+vzm7frt8vrl7fnp8Prr8frx9fvn7/r2+fzc6+bP49s6llxgrn1irn5lsYFxt4sxkVMuh001lFc5kllCm2E9j1pJoGhFlWFNn2pRpG9VonBSnG1QmGpbqnhfrHtcpHZZmXBosoNttIdHnWR7dE9+dU57c1CHekyDd02Rf0iMfUqdhUWXgkawjz6sjECnikKiiEO0kD3///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABdGX5SAAAANnRSTlP//////////////////////////////////////////////////////////////////////wChj04xAAAAAWJLR0Q/PmMwdQAAAAlwSFlzAAAASAAAAEgARslrPgAAAIdJREFUGNNly9cOgjAARuFSW2mhdaLiwq3IHvr+r6b8JUjiufySQ97fXr0IYNBloCAzRUwFoHRvyjWVgIqOVRNVtALUMjieJtOrDGQNyKyd1ZYBcr7WTVzzHJAIf3Xf7s/CFwkgtUd2WwqInAUWRzsRIGbefLk5XJjHYkA47AoBz8cvQL8/+ADxvyAVIcVyfQAAAABJRU5ErkJggg==);"
+ "	background-repeat: no-repeat;"
+ "}"
+ ".nunit-hidden{"
+ "	display:none;"
+ "}"
;
	var totalTest = 0, totalAssert = 0 ; totalFailed = 0,results = [];
	var row_template = "<span class='nunit-single-test'>${testName}</span>${time}<span>";
	var reporter = {
		testUnitBegin: function(testCount, desc){
			totalTest += testCount ;
			results.push([desc]);
		},
		testBegin: function(id, testName){

		},
		testEnd: function(id, testName, result){
			totalAssert += result.assertCount;
			results.push([testName, result.passed, result.assertCount, result.endTime - result.startTime, result.error]);
		},

		testUnitEnd: function(testCount, desc, failedCount){
			totalFailed += failedCount;

		},

		println: function(level, message){

		}

	};

	var addClass = function(dom, className){
		var cls = dom.className || "";
		if(cls.indexOf(className) == -1){
			cls += className ;
		}
		dom.className = cls ;
	};

	var removeClass = function(dom, className){
		var cls = dom.className || "";
		if(cls.indexOf(className) != -1){
			cls = cls.replace(className, "") ;
		}
		dom.className = cls ;
	};

	var hasClass = function(dom, className){
		var cls = dom.className || "";
		if(cls.indexOf(className) != -1){
			return true ;
		}
		return false ;
	};

	var setStyle = function(dom, styleText){
		dom.style.cssText = styleText ;
	}
	var run = function(){
		nunit.execute({reporters:[reporter], debug:true});	
	};

	var makeRow = function(){
		var span = document.createElement("div");
		span.className = span.className || "";
		span.className += " nunit-single-test";

		return span ;
	};
	var showErr = function(dom, error){
		removeClass(dom, "nunit-hidden");
		dom.innerHTML = "<span class='nunit-error-msg'>" + error.message + "</span><br> See console for stacktrace.";
		if(console){
			throw error ;
		}
	};

	var clearErr = function(dom){
		addClass(dom, "nunit-hidden");
		dom.innerHTML = "";
	};

	var updateView = function(){
		var summary = document.getElementById("nunitSummary");
		var err = document.getElementById("nunitError");
		summary.className = totalFailed? "nunit-any-failed": "nunit-all-pass";
		document.getElementById("totalTest").innerHTML = totalTest;
		document.getElementById("totalPassed").innerHTML = totalTest - totalFailed;
		document.getElementById("totalAssert").innerHTML = totalAssert;
		//update detail
		var detail = document.getElementById("nunitDetail") ;
		for(var i = 0 ; i < results.length ; i ++ ){
			var result = results[i];
			var div = makeRow();
			switch(result.length){
				case 1:
					div.innerHTML = "NUnit.Test: " + result[0];
					div.className += " test-name";
					break ;
				case 5:
					div.className += result[1] ? " passed": " failed";
					div.innerHTML = result[0] + " (" + (result[3]? result[3]/1000: "0.000") + "s)"
					// detail.appendChild(document.createElement("br"));
					break;
			}
			detail.appendChild(div);
			(function(div, result){
				div.onclick = function(){
					clearErr(err);
					
				};
				if(result[4]){
					div.onclick = function(){
						showErr(err, result[4]);
					}; 
				}
			})(div, result);
			
		}
		clearErr(err);
		if(totalFailed > 0) removeClass(detail, "nunit-hidden");
	};

	var setStyle = function(dom, styleText){
		dom.style.cssText = styleText ;
	};
	var prepareDOM = function(){
		/*
		<div id="nunitReport">
				<div id="nunitSummary" class="all-pass">
					<span id="totalPassed">0</span>/<span id="totalTest">0</span> (<span id="totalAssert"></span> asserts.)
				</div>
				<div id="nunitDetail">
					<!--<span class='single-test failed'>${testName} ${time}<span>-->
				</div>
		</div>
		*/
		var inner = "<div id=\"nunitSummary\" class=\"nunit-all-pass\"><span id=\"totalPassed\">0</span>/<span id=\"totalTest\">0</span> (<span id=\"totalAssert\"></span> asserts.)</div><div id=\"nunitDetail\"></div><div id=\"nunitError\">Errors</div>";
		var div = document.createElement("div");
		div.id = "nunitReport";
		div.innerHTML = inner ;
		// setStyle(div, "font-family: Arial, Helvetica, sans-serif;min-width: 200px;max-width: 200px;width: 200px;position: fixed;right: 3px;bottom: 168px;");
		var body = document.body;
		body.insertBefore(div, body.firstChild);
		// setStyle(document.getElementById("nunitSummary"), "height: 20px;max-height: 20px;min-height: 20px;border:1px ridge #808080 ;padding: 4px 0 0 4px;font-size: small;");
		// setStyle(document.getElementById("nunitDetail"), "max-height: 200px;min-height: 0px;overflow-y: scroll;	border:1px inset #808080 ;border-top: 0;padding: 3px 0 0 3px;resize: vertical;");
	};
	var prepareCSS = function(){
		var style = document.createElement("style");
		style.type = "text/css";
		if (style.styleSheet) {
		    style.styleSheet.cssText = styleText;
		  } else {
		    style.appendChild(document.createTextNode(styleText));
		  }
		document.getElementsByTagName("head")[0].appendChild(style)	
		// document.head.appendChild(style);
	};

	var init = function(){
		//init
		prepareCSS();
		prepareDOM();

		var sum = document.getElementById("nunitSummary");
		var detail = document.getElementById("nunitDetail") ;
		sum.onclick = function(event){
			//detail.nunit-hidden = !detail.nunit-hidden ;
			// detail.className = detail.className.indexOf("closing") > -1 ? detail.className.replace("closing", "opening") : detail.className + " closing";
			if(hasClass(detail, "nunit-hidden")){
				removeClass(detail, "nunit-hidden");
			}else{
				addClass(detail, "nunit-hidden");
			}
		}
		addClass(detail, "nunit-hidden");
	};
	
	if ( document.addEventListener ) {
		//console.log("addEventListener !");
		window.addEventListener( "load", function(){
			init();
			run();
			updateView();
		}, false );

	// If IE event model is used
	} else {
		//console.log("attachEvent !");
		// A fallback to window.onload, that will always work
		window.attachEvent( "onload", function(){
			init();
			run();
			updateView();
		});

	}
	

})(window.NUnit);
