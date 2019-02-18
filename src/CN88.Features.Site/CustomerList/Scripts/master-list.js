var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MasterList = /** @class */ (function (_super) {
    __extends(MasterList, _super);
    function MasterList($, statusComponent, numeral, pager) {
        var _this = _super.call(this, $, statusComponent, numeral, pager, "Master") || this;
        _this.renderMasterList = function (rs) {
            _this.$tblReport.removeClass("display-none");
            var html = "";
            var customerIndex = _this.reportParam.PageSize * (_this.reportParam.PageIndex - 1) + 1;
            if (rs.Customers.length == 0) {
                html = "<tr><td colspan='7' align='center' class='no-info'>" + _this.viewResources.NoInformationIsAvailable + "</td></tr>";
            }
            else {
                for (var i = 0; i < rs.Customers.length; i++, customerIndex++) {
                    var customer = rs.Customers[i];
                    html += "<tr class='row-customer " + _this.indexManager.getCustomerColor(customer) + "' cust-id='" + customer.CustId + "'>" +
                        "<td class='text-center'>" + customerIndex + "</td>" +
                        "<td class='view-downline' cust-id='" + customer.CustId + "'>" + customer.Username + "</td>" +
                        "<td>" + customer.Name + "</td>" +
                        "<td class='text-right update-credit' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + _this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                        "<td class='text-center'>" + customer.CreationDateStr + "</td>" +
                        "<td class='text-center update-status' cust-id='" + customer.CustId + "' username='" + customer.Username + "' current-status='" + customer.Status + "'>" + _this.indexManager.getStatusName(customer.Status) + "</td>" +
                        "<td class='text-center'>" +
                        "<span class='icon-edit' cust-id='" + customer.CustId + "' username='" + customer.Username + "' " + (_this.HasUpdatePermission ? "" : "disabled='disabled' title='" + _this.indexManager.viewResources.Disabled + "'") + "></span>" +
                        "<span class='icon-add' cust-id='" + customer.CustId + "' username='" + customer.Username + "' " + (_this.HasAddNewPermission ? "" : "disabled='disabled' title='" + _this.indexManager.viewResources.Disabled + "'") + "></span>" +
                        "</td>" +
                        "</tr>";
                }
            }
            _this.$tblReport.find("> tbody").empty().html(html);
            _this.$(window).trigger('resize.stickyTableHeaders');
            _super.prototype.bindReportEvents.call(_this, rs);
        };
        _this.reloadState = function (reportParam) {
            _this.$username.val(reportParam.Username);
            _this.$dropdownStatus.val(reportParam.Status);
            _this.reportParam.PageSize = reportParam.PageSize;
            _this.reportParam.PageIndex = reportParam.PageIndex;
            _this.$("#PageIndexMaster").val(reportParam.PageIndex);
            _this.$("#PageSizeMaster").val(reportParam.PageIndex);
            _this.viewList();
        };
        return _this;
    }
    MasterList.prototype.init = function (viewResources, indexManager) {
        _super.prototype.init.call(this, viewResources, indexManager, this.renderMasterList, siteInfrastructure.roleId.master, indexManager.viewMaster, indexManager.addAgentForMaster);
    };
    return MasterList;
}(BaseList));
//# sourceMappingURL=master-list.js.map