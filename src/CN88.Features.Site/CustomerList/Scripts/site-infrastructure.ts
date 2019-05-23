"use strict";

declare var numeral: any;

class SiteInfrastructure {
    pageList = [50, 100, 200, 500];

    customerStatus = {
        all: 0,
        active: 4,
        viewOnly: 3,
        suspended: 2,
        inactive: 1
    };

    roleId = {
        master: 3,
        agent: 2,
        member: 1
    };

    localStorage = {
        get: (key: string) => {
            var result = store.enabled ? store.get(key) : $.cookie(key);
            return result ? JSON.parse(result) : undefined;
        },

        set: (key: string, value: any) => {
            var data = JSON.stringify(value);
            store.enabled ? store.set(key, data) : $.cookie(key, data, this.localStorage.getCookieExpireOptions());
        },

        check: (key: string): boolean => {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },

        getCookieExpireOptions: () => {
            var cookieExpire: any = { expires: 365, path: window.location.pathname };

            return cookieExpire;
        }
    };
}

const siteInfrastructure = new SiteInfrastructure();