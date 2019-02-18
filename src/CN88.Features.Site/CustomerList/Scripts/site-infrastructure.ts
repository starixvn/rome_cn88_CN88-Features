"use strict";

declare var numeral: any;
interface JQuery {
    stickyTableHeaders(): void;
}

class SiteInfrastructure {
    pageList = [50, 100, 200, 500];

    customerStatus = {
        all: 0,
        open: 1,
        closed: 3,
        suspended: 2
    };

    closeReasonConstant = {
        LogInFailedOrExpired: 0,
        SecurityCode: 3,
        Upline: 2,
        Admin: 1
    };

    roleId = {
        master: 3,
        agent: 2,
        member: 1
    };

    localStorage = {
        get: (key: string) => {
            return store.enabled ? store.get(key) : $.cookie(key);
        },

        set: (key: string, value: any) => {
            store.enabled ? store.set(key, value) : $.cookie(key, value, this.localStorage.getCookieExpireOptions());
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