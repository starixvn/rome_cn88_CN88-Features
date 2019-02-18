"use strict";
var SiteInfrastructure = /** @class */ (function () {
    function SiteInfrastructure() {
        var _this = this;
        this.pageList = [50, 100, 200, 500];
        this.customerStatus = {
            all: 0,
            open: 1,
            closed: 3,
            suspended: 2
        };
        this.closeReasonConstant = {
            LogInFailedOrExpired: 0,
            SecurityCode: 3,
            Upline: 2,
            Admin: 1
        };
        this.roleId = {
            master: 3,
            agent: 2,
            member: 1
        };
        this.localStorage = {
            get: function (key) {
                return store.enabled ? store.get(key) : $.cookie(key);
            },
            set: function (key, value) {
                store.enabled ? store.set(key, value) : $.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            check: function (key) {
                return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                return cookieExpire;
            }
        };
    }
    return SiteInfrastructure;
}());
var siteInfrastructure = new SiteInfrastructure();
//# sourceMappingURL=site-infrastructure.js.map