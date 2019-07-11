"use strict";
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
        super: 4,
        master: 3,
        agent: 2,
        member: 1
    };
}

const siteInfrastructure = new SiteInfrastructure();