HTMLElement.prototype.addElement = function (arrayName, element) {
    var array = this[arrayName] = this[arrayName] || [];
    array.push(element);
    this.appendChild(element);
    return this;
};

HTMLElement.prototype.removeElement = function(arrayName, element) {
    var array = this[arrayName] = this[arrayName] || [];
    array.splice(array.indexOf(element), 1);
    this.removeChild(element);
    return this;
};

HTMLElement.prototype.removeAllElements = function (arrayName) {
    var array = this[arrayName] = this[arrayName] || [];
    for (var i = 0; i < array.length; i++) {
        this.removeChild(array[i]);
    }
    array.length = 0;
    return this;
};

HTMLElement.prototype.setElement = function (name, element) {
    this[name] = element;
    this.appendChild(element);
    return this;
};

HTMLElement.prototype.deleteElement = function (name) {
    if (this[name]) {
        this.removeChild(this[name]);
        delete this[name];
    }
    return this;
};

window.alert = function (message) {
    new Notify({ text: message });
};

Array.prototype.indexOf = function(obj, start) {
    for (var i = (start || 0), j = this.length; i < j; i++) {
        if (this[i] === obj) {
            return i;
        }
    }
    return -1;
};
