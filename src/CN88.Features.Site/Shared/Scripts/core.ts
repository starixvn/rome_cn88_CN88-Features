"use strict";
interface Window {
    $: JQueryStatic;
}

class CN88Features {
    $: JQueryStatic;
    store: StoreJSStatic;

    constructor($: JQueryStatic, store: StoreJSStatic) {
        this.$ = $;
        this.store = store;
    }

    format = {
        monthFormat: "MMMM - yyyy",
        shortDateWithSlash_DatePicker: "MM/DD/YYYY",
        shortDateWithSlash_MonthPicker: "MM/dd/yyyy",
        shortDate_DatePicker: "YYYY-MM-DD"
    };

    keyCode = {
        enter: 13
    };

    localStorage = {
        get: (key: string) => {
            const result = this.store.enabled ? this.store.get(key) : this.$.cookie(key);
            return result ? JSON.parse(result) : undefined;
        },

        set: (key: string, value) => {
            const data = JSON.stringify(value);
            this.store.enabled ? this.store.set(key, data) : this.$.cookie(key, data, this.localStorage.getCookieExpireOptions());
        },

        getCookieExpireOptions: () => {
            let cookieExpire: any = { expires: 365, path: window.location.pathname };

            if (this.isIe()) {
                cookieExpire = { expires: 365 };
            }

            return cookieExpire;
        }
    };

    isIe = () => {
        const ua = window.navigator.userAgent;
        const msie = ua.indexOf("MSIE ");

        return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
    };

    replaceQueryStringValue = (queryString: string, paramName: string, paramValue) => {
        if (typeof queryString === "undefined"
            || queryString === null) {
            return "";
        }

        const regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");

        const newQueryString = regex.test(queryString)
            ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
            : queryString + (queryString.indexOf("?") >= 0 ? "&" : "?") + paramName + "=" + paramValue;

        return newQueryString;
    };

    convertAttributeToBool = (attrValue: string) => {
        return !!attrValue && attrValue.toString().toLowerCase() === "true";
    };
}

const cn88Features = new CN88Features($, store);