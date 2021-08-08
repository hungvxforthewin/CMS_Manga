//Number
var opNumber = {

    digitGroupSeparator: ',',
    digitalGroupSpacing: 4,
    maximumValue: '99999999999999',
    minimumValue: '-999999999999',
    decimalCharacter: '.',
    decimalPlaces: 0
};
//Negative Integer
var opNegInt = {

    digitGroupSeparator: '.',
    digitalGroupSpacing: 3,
    maximumValue: '0',
    minimumValue: '-999999999999',
    decimalCharacter: ',',
    decimalPlaces: 0
};
//Positive Integer
var opPosInt = {
    digitGroupSeparator: '.',
    digitalGroupSpacing: 3,
    minimumValue: '0',
    maximumValue: '999999999999',
    decimalCharacter: ',',
    decimalPlaces: 0
};
var opPosInt2 = {
    digitalGroupSpacing: 3,
    minimumValue: '0',
    maximumValue: '999999999999',
    decimalPlaces: 0
};
var opMoney = {

    digitGroupSeparator: ',',
    digitalGroupSpacing: 3,
    maximumValue: '99999999999999',
    minimumValue: '-999999999999',
    decimalCharacter: '.',
    decimalPlaces: 0
};

var opPercent = {

    digitGroupSeparator: '.',
    digitalGroupSpacing: 3,
    maximumValue: '100.00',
    minimumValue: '0.00',
    decimalCharacter: ',',
    decimalPlaces: 0
};

var formatNumber = function (value) {
    try {
        return AutoNumeric.format(value, opNumber);
    } catch (e) {
        return value;
    }
    
};
var formatPosInt2 = function (value) {
    try {
        return AutoNumeric.format(value, opPosInt2);
    } catch (e) {
        return value;
    }
};
var formatPosInt = function (value) {
    try {
        return AutoNumeric.format(value, opPosInt);
    } catch (e) {
        return value;
    }
};
var formatNegInt = function (value) {
    try {
        return AutoNumeric.format(value, opNegInt);
    } catch (e) {
        return value;
    }
};

var formatMoney = function (value) {
    try {
        return AutoNumeric.format(value, opMoney);
    }
    catch{
        return value;
    }
    
};

var formatPercent = function (value) {
    try {
        return AutoNumeric.format(value, opPercent);
    } catch (e) {
        return value;
    }
    
};

var getNumber_AutoNumeric = function (value) {
    return AutoNumeric.unformat(value, opNumber);
};
var getPosInt_AutoNumeric = function (value) {
    return AutoNumeric.unformat(value, opPosInt);
};
var getPosInt2_AutoNumeric = function (value) {
    return AutoNumeric.unformat(value, opPosInt2);
};
var getNegInt_AutoNumeric = function (value) {
    return AutoNumeric.unformat(value, opNegInt);
};
var getMoney_AutoNumeric = function (value) {
    return AutoNumeric.unformat(value, opMoney);
};
var getPercent_AutoNumeric = function (value) {
    return AutoNumeric.unformat(value, opPercent);
};