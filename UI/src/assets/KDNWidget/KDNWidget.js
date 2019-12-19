(function() {
    var kdnOptions = {
        baseUrl: "http://www.kdniao.com/JSInvoke/"
    };
    var utilityService = new KDNUtilitiesService();
    function KDNSearchResultService(options) {
        this.param = options || {};
        this.validateData = function() {
            if (!utilityService.hasProperty(this.param, "expCode") || !utilityService.hasProperty(this.param, "expNo")) return false;
            if (!utilityService.hasProperty(this.param, "sortType")) this.param.sortType = "DESC";
            if (!utilityService.hasProperty(this.param, "color")) this.param.color = "rgb(46,114,251)";
            if (this.param.color.substring(0, 3) != "rgb" || this.param.color.substring(0, 3) != "RGB") this.param.color = "rgb(46,114,251)";
            return true
        };
        this.excuteProcess = function() {
            window.location.href = kdnOptions.baseUrl + "MSearchResult.aspx?expCode=" + this.param.expCode + "&expNo=" + this.param.expNo + "&backUrl=" + window.location.href + "&color=" + this.param.color
        }
    }
    function KDNPCSearchResultService(options) {
        this.param = options || {};
        this.validateData = function() {
            if (!utilityService.hasProperty(this.param, "expCode") || !utilityService.hasProperty(this.param, "expNo") || !utilityService.hasProperty(this.param, "container")) return false;
            return true
        };
        this.excuteProcess = function() {
            var _containerID = document.getElementById((this.param.container || ""));
            var _showType = "normal";
            if (this.param.showType == "pop") _showType = "pop";
            var iframeSrc = kdnOptions.baseUrl + "SearchResult.aspx?expCode=" + this.param.expCode + "&expNo=" + this.param.expNo;
            var iframeElement = document.createElement("iframe");
            iframeElement.setAttribute("width", "900");
            iframeElement.setAttribute("height", "550");
            iframeElement.setAttribute("border", "0");
            iframeElement.setAttribute("frameborder", "0");
            iframeElement.setAttribute("scrolling", "no");
            iframeElement.setAttribute("src", iframeSrc);
            if (_showType == "normal") {
                _containerID.innerHTML = "";
                _containerID.style.display = "block";
                _containerID.appendChild(iframeElement)
            } else {
                _containerID.innerHTML = "";
                _containerID.style.display = "block";
                _containerID.setAttribute("class", "kdnlogin-box");
                var myhtml = "<div class='kdnlogin-title'>即时查询结果<div class='kdnlogin-close'>关闭</div></div><div class='kdnlogin-content'><iframe width='900' height='550' frameborder='0' scrolling='auto' src='" + iframeSrc + "'> </iframe></div>";
                _containerID.innerHTML = myhtml;
                utilityService.validateReg.addEvent(document.getElementsByClassName("kdnlogin-close")[0], "click",
                function() {
                    _containerID.style.removeProperty("display");
                    _containerID.style.display = "none";
                    _containerID.innerHTML = ""
                },
                this)
            }
        }
    }
    function KDNSearchTrackService(options) {
        this.param = options || {};
        this.validateData = function() {
            if (!utilityService.hasProperty(this.param, "sortType")) this.param.sortType = "DESC";
            if (!utilityService.hasProperty(this.param, "color")) this.param.color = "rgb(46,114,251)";
            return true
        };
        this.excuteProcess = function() {
            window.location.href = kdnOptions.baseUrl + "MSearchTrack.aspx?backUrl=" + window.location.href + "&sortType=" + this.param.sortType + "&color=" + this.param.color
        }
    }
    function KDNPCSearchTrackService(options) {
        this.param = options || {};
        this.validateData = function() {
            if (!utilityService.hasProperty(this.param, "container")) {
                return false
            }
            return true
        };
        this.excuteProcess = function() {
            var targetContainer = document.getElementById(this.param.container);
            if (!targetContainer) {
                console.error("请传入一个容器div");
                return false
            }
            targetContainer.style.width = "1015px";
            targetContainer.style.height = "100%";
            targetContainer.style.margin = "0px auto";
            var iframeHtml = "<iframe frameborder='0' width='1012' height='' scrolling='auto' src='" + kdnOptions.baseUrl + "SearchTrack.aspx'></iframe>";
            
            targetContainer.innerHTML = iframeHtml
        }
    }
    function KDNOnlineOrderService(options) {
        this.param = options || {};
        this.validateData = function() {
            if (!utilityService.hasProperty(this.param, "sortType")) this.param.sortType = "DESC";
            if (!utilityService.hasProperty(this.param, "color")) this.param.color = "rgb(46,114,251)";
            return true
        };
        this.excuteProcess = function() {
            window.location.href = kdnOptions.baseUrl + "MOnlineOrder.aspx?backUrl=" + window.location.href + "&sortType=" + this.param.sortType + "&color=" + this.param.color
        }
    }
    function KDNPCOnlineOrderService(options) {
        this.param = options || {};
        this.validateData = function() {
            if (!utilityService.hasProperty(this.param, "container")) {
                return false
            }
            return true
        };
        this.excuteProcess = function() {
            var targetContainer = document.getElementById(this.param.container);
            if (!targetContainer) {
                console.error("请传入一个容器div");
                return false
            }
            targetContainer.style.width = "1015px";
            targetContainer.style.height = "705px";
            targetContainer.style.margin = "0px auto";
            var iframeHtml = "<iframe frameborder='0' width='1012' height='603' scrolling='auto' src='" + kdnOptions.baseUrl + "OnlineOrder.aspx'></iframe>";
            targetContainer.innerHTML = iframeHtml
        }
    }
    function KDNUtilitiesService() {
        this.hasProperty = function(object, property) {
            return object.hasOwnProperty(property) || (property in object)
        };
        this.validateReg = {
            checkNum: function(targetValue) {
                return new RegExp("^[0-9]*$").test(targetValue)
            },
            addEvent: function(elm, evType, fn, useCapture) {
                if (elm.addEventListener) {
                    elm.addEventListener(evType, fn, useCapture);
                    return true
                } else if (elm.attachEvent) {
                    var r = elm.attachEvent('on' + evType, fn);
                    return r
                } else {
                    elm['on' + evType] = fn;
                }
            }
        }
    }
    function Widget(options) {
        this.customerOptions = options || {};
        this.validateOptions = function() {
            var _customerOptions = this.customerOptions;
            var _utilitiesService = this.utilitiesService;
            if ((typeof _customerOptions) === 'object' && _utilitiesService.hasProperty(_customerOptions, "serviceType")) {
                var selectType = _customerOptions.serviceType || "";
                switch (selectType) {
                case "A":
                    this.currentService = new KDNSearchResultService(_customerOptions);
                    break;
                case "B":
                    this.currentService = new KDNPCSearchResultService(_customerOptions);
                    break;
                case "C":
                    this.currentService = new KDNSearchTrackService(_customerOptions);
                    break;
                case "D":
                    this.currentService = new KDNPCSearchTrackService(_customerOptions);
                    break;
                case "E":
                    this.currentService = new KDNOnlineOrderService(_customerOptions);
                    break;
                case "F":
                    this.currentService = new KDNPCOnlineOrderService(_customerOptions);
                    break;
                default:
                    console.error("没有此类型的服务，请对照快递鸟官网（http://www.kdniao.com/）相关api修改后重试");
                    break
                }
                this.isValidateParam = this.currentService.validateData()
            } else {
                console.error("调用快递鸟外部调用功能缺少必要参数，请对照快递鸟官网（http://www.kdniao.com/）相关api修改后重试")
            }
            return this
        };
        this.isValidateParam = false;
        this.currentService = null;
        this.utilitiesService = new KDNUtilitiesService();
        this.runer = function() {
            if (this.isValidateParam) {
                this.currentService.excuteProcess()
            }
            return this
        }
    }
    var api = {
        run: function(opts) {
            var w = new Widget(opts);
            w.validateOptions().runer()
        }
    };
    this.KDNWidget = api
})();