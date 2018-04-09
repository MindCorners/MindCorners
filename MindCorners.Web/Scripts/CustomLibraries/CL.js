CL = {
    generateGuid: function () {

        function S4() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        }

        return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
    },

    merge: function (a, b) {
        var copy = {};
        CL.extend(copy, a, [HTMLElement, Array]);
        CL.extend(copy, b, [HTMLElement, Array]);
        return copy;
    },

    extend: function extend(a, b, excludeInstances) {
        for (var prop in b)
            if (b.hasOwnProperty(prop)) {
                var isInstanceOfExcluded = false;
                if (excludeInstances)
                    for (var i = 0; i < excludeInstances.length; i++)
                        if (b[prop] instanceof excludeInstances[i])
                            isInstanceOfExcluded = true;

                if (typeof b[prop] === 'object' && !isInstanceOfExcluded) {
                    a[prop] = a[prop] !== undefined ? a[prop] : {};
                    extend(a[prop], b[prop], excludeInstances);
                } else
                    a[prop] = b[prop];
            }
    },

    addEvent: function (element, event, fn) {
        if (element.addEventListener)
            element.addEventListener(event, fn, false);
        else if (element.attachEvent)
            element.attachEvent('on' + event, fn);
    },

    removeEvent: function (element, event, fn) {
        if (element.removeEventListener)
            element.removeEventListener(event, fn, false);
        else if (element.detachEvent)
            element.detachEvent('on' + event, fn);
    },

    callOneFunction: function (array, fnName, args) {
        for (var i = 0; i < array.length; i++) {
            var o = array[i];
            o[fnName].apply(o, args);
        }
    },

    convertCSharpDateTimeToJSDate: function (cSharpDateTime) {
        var re = /-?\d+/;
        var m = re.exec(cSharpDateTime);
        return new Date(parseInt(m[0]));
    },

    convertCSharpDateTimeToDateString: function (cSharpDateTime) {
        if (cSharpDateTime) {
            var date = this.convertCSharpDateTimeToJSDate(cSharpDateTime);
            var d = date.getDate();
            var m = date.getMonth() + 1;
            var y = date.getFullYear();
            return (d <= 9 ? '0' + d : d) + '/' + (m <= 9 ? '0' + m : m) + '/' + y;
        }
        return '';
    },

    fillElementValues: function (data, prefix) {
        for (var prop in data) {
            var element = document.getElementsByName(prefix ? prefix + '.' + prop : prop)[0];
            element.value = data[prop];
            jQuery(element).valid();
        }
    },

    createHiddenInput: function (name, value, container) {
        var hidden = document.createElement('input');
        hidden.type = 'hidden';
        hidden.name = name;
        hidden.value = value;
        container.addElement('hiddens', hidden);
    },

    createHiddenInputWithId: function (name, value, container, id) {
        var hidden = document.createElement('input');
        hidden.type = 'hidden';
        hidden.id = id;
        hidden.name = name;
        hidden.value = value;
        container.addElement('hiddens', hidden);
    },

    getDateDdmmyy: function (value) {
        if (value == null)
            return null;
        return jQuery.datepicker.parseDate("dd/mm/yy", value);
    },

    limitString: function (str, length) {
        return str.length > length ? str.substr(0, length) + '...' : str;
    },

    asyncCall: function () {
        var self = this;
        var fns = [];

        self.completed = 0;

        self.addFn = function (fn, args, thisArg) {
            fns.push({ fn: fn, args: args, thisArg: thisArg });
        };

        self.run = function () {
            for (var i = 0; i < fns.length; i++) {
                (function (j) {
                    setTimeout(function () {
                        var fn = fns[j],
                            args = fn.args || [],
                            thisArg = fn.thisArg || window;

                        if (j < fns.length) {
                            fn.fn.apply(thisArg, args);
                            if (++self.completed === fns.length)
                                self.done();
                        }
                    }, 0);
                })(i);
            }
        };

        self.done = function () {
            console.log('[asyncCall].done not handled');
        };
    },

    htmlDecode: function (str) {
        var tempDiv = document.createElement('div');
        tempDiv.innerHTML = str;
        return tempDiv.innerText;
    },

    htmlEncode: function (str) {
        var tempDiv = document.createElement('div');
        tempDiv.innerText = str;
        return tempDiv.innerHTML;
    },

    generateFormatError: function (allowedFormatsString, culture) {
        var allowedFormats = allowedFormatsString.split('|');
        var result = (culture === 'ka-ge' ? 'მხოლოდ ' : 'Only ');
        for (var i = 0; i < allowedFormats.length; i++) {
            if (i === allowedFormats.length - 2)
                result += allowedFormats[i];
            else if (i === allowedFormats.length - 1)
                result += (culture === 'ka-ge' ? ' ან ' : ' or ') + allowedFormats[i] +
                    (culture === 'ka-ge' ? ' ფაილის ფორმატი არის ნებადართული' : ' files are allowed');
            else
                result += allowedFormats[i] + ', ';
        }
        return result;
    },

    // tu attributeValue == undefined mashin wamoigebs im elementebs romlebsac aqvt attributeName atributi
    getElelementsByAttribute: function (tagName, attributeName, attributeValue, parentElement) {
        parentElement = parentElement || document;
        var result = [];
        var elements = parentElement.getElementsByTagName(tagName);
        for (var i = 0; i < elements.length; i++) {
            var el = elements[i];
            var val = el.getAttribute(attributeName);
            if (attributeValue == undefined || val == attributeValue)
                result.push(el);
        }
        return result;
    },

    addScript: function (src, callback) {
        var head = document.getElementsByTagName('head')[0];
        var existings = CL.getElelementsByAttribute('script', 'src', src, head);
        var hasCallback = typeof callback === 'function';

        if (existings.length === 0) {
            var script = document.createElement('script');
            script.setAttribute('type', 'text/javascript');
            script.setAttribute('src', src);
            if (hasCallback)
                CL.addEvent(script, 'load', callback);
            head.appendChild(script);
        } else if (hasCallback)
            callback.call(existings[0]);
    },

    addStyle: function (src, callback) {
        var head = document.getElementsByTagName('head')[0];
        var existings = CL.getElelementsByAttribute('link', 'href', src, head);
        var hasCallback = typeof callback === 'function';

        if (existings.length === 0) {
            var style = document.createElement('link');
            style.setAttribute('rel', 'stylesheet');
            style.setAttribute('type', 'text/css');
            style.setAttribute('href', src);
            if (hasCallback)
                CL.addEvent(style, 'load', callback);
            head.appendChild(style);
        } else if (hasCallback)
            callback.call(existings[0]);
    },

    createStyles: function (datas) {
        var head = document.getElementsByTagName('head')[0];
        var style = document.createElement('style');
        style.type = 'text/css';
        for (var i = 0; i < datas.length; i++) {
            var data = datas[i],
                selector = data.selector,
                params = data.params;

            style.appendChild(document.createTextNode(selector + '{'));
            for (var prop in params)
                style.appendChild(document.createTextNode(prop + ':' + params[prop] + ';'));
            style.appendChild(document.createTextNode('}'));
        }
        head.appendChild(style);
    },

    addParameterToUrl: function (url, name, value) {
        var containsQ = url.indexOf('?') !== -1;
        return (url + (containsQ ? '&' : '?') + name + '=' + value);
    },

    print: function (content, callback) {
        var iframe = document.createElement('iframe');
        CL.extend(iframe.style, {
            width: '1px',
            height: '1px'
        });
        document.body.appendChild(iframe);
        var iframeWindow = iframe.contentWindow;

        var scriptTag = iframeWindow.document.createElement("script");
        scriptTag.type = "text/javascript";
        script = iframeWindow.document.createTextNode('function Print(){ window.print(); }');
        scriptTag.appendChild(script);

        if (typeof content === 'string')
            iframeWindow.document.body.innerHTML = content;
        else if (typeof content === 'object')
            iframeWindow.document.body.appendChild(content);
        iframeWindow.document.body.appendChild(scriptTag);

        // temp
        //        var images = iframeWindow.document.getElementsByTagName('img');
        //        var itemsLeft = images.length;

        //        if (itemsLeft > 0)
        //            for (var i = 0; i < images.length; i++)
        //                images[i].addEventListener('load', function () {
        //                    if (--itemsLeft === 0)
        //                        _print();
        //                });
        //        else
        _print();

        function _print() {
            iframeWindow.Print();
            iframe.parentNode.removeChild(iframe);
            callback();
        }
    },

    array: {
        union: function (ar1, ar2) {
            var results = [];
            for (var i = 0; i < ar1.length; i++) {
                results.push(ar1[i]);
            }

            for (var i = 0; i < ar2.length; i++) {
                var current = ar2[i];
                if (!results.some(function (result) { return result === current; }))
                    results.push(current);
            }
            return results;
        },

        filter: function (arr, fn) {
            var newArr = [];
            for (var i = 0; i < arr.length; i++) {
                var el = arr[i];
                if (fn.call(el, el))
                    newArr.push(el);
            }
            return newArr;
        }
    },

    url: {
        construct: function (ob) {
            var ps = [];
            for (var prop in ob) {
                ps.push(prop + '=' + ob[prop]);
            }
            return ps.join('&');
        },
        appendSearch: function (url, searchString) {
            var hasQ = url.indexOf('?') !== -1;
            return url + (hasQ ? '&' : '?') + searchString;
        },
        append: function (url, search) {
            var s = '';
            if (typeof search === 'object')
                s = CL.url.construct(search);
            else if (typeof search === 'string')
                s = search;
            else
                throw 'search parameter type must be either string or object';
            return CL.url.appendSearch(url, s);
        }
    }
};
