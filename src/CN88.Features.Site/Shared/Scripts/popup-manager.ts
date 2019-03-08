interface JQueryStatic {
    clone: any
}

declare var bootbox: any;

class PopupManager {
    protected $: JQueryStatic;
    protected topWindow: any;

    positionPadding = 5;
    activePopups = [];
    onClosing = null;

    popupId = "";

    // Choose most suitable screen resolution 1920 X 1080
    defaultOptions = {
        height: 1000,
        width: 480,
        title: "",
        popupCss: "",
        handlerId: null,
        backdrop: true,
        centerScreen: true
    };

    init = ($: JQueryStatic, topWindow: any) => {
        this.$ = $;
        this.topWindow = topWindow;
    };
    leftPosition = () => {
        return this.positionPadding;
    };
    topPosition = () => {
        // top-left main frame
        return 74 + this.positionPadding;
    };
    closeAllPopup = () => {
        this.topWindow.$('.modal[id^="page-popup-"]').modal("hide");
        this.activePopups = [];
    };
    close = () => {
        var popUp = this.activePopups.pop();
        if (!popUp) {
            return;
        }

        popUp.find('.modal-body #target').remove();
        popUp.modal("hide");

        if (this.onClosing && typeof (this.onClosing) == "function") {
            this.onClosing();
        }
    };
    setDefaultOptions = (options: any) => {
        options.centerScreen = options.centerScreen == undefined || options.centerScreen;
        options.left = options.left || this.leftPosition();
        options.top = options.top || this.topPosition();

        options.width = options.width || this.defaultOptions.width;
        options.height = options.height || this.defaultOptions.height;
        options.title = options.title || this.defaultOptions.title;

        options.backdrop = options.backdrop == undefined || options.backdrop;
        options.handlerId = options.handlerId || "mainPopup";
    };
    calculateWidthHeight = (options) => {
        var realHeight = options.height;
        var maxWidth = this.topWindow.innerWidth;

        var minHeight = 250;
        var maxHeight = this.topWindow.innerHeight - options.top - 60;
        maxHeight = Math.max(maxHeight, minHeight);

        var realWidth = Math.min(options.width, maxWidth);
        realHeight = Math.min(realHeight, maxHeight);
        realHeight = Math.max(realHeight, minHeight);

        return {
            width: realWidth,
            height: realHeight
        };
    };
    getScrollLeft = (element) => {
        return element.pageXOffset || element.scrollX || element.scrollLeft || 0;
    };
    getScrollTop = (element) => {
        return element.pageYOffset || element.scrollY || element.scrollTop || 0;
    };
    isiOS = () => {
        var isiOS = /iPhone|iPad|iPod/i.test(navigator.userAgent);
        return isiOS;
    };
    isSmallDevice = () => {
        var isSmallDevice = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        return isSmallDevice;
    };
    getElementSize = (element) => {
        var style = element[0].style;
        return {
            width: parseInt(style.width.substring(0, style.width.length - 2)),
            height: parseInt(style.height.substring(0, style.height.length - 2))
        };
    };
    setIframeInfo = (iframe, popUp, iframeOptions) => {
        iframe.css({
            width: '100%',
            height: '100%',
            left: iframeOptions.left,
            top: iframeOptions.top
        });

        iframe.load(() => {
            this.updatePopupTitleAfterLoad(popUp, iframe);

            if (this.isSmallDevice()) {
                // This will make elements on popup lose focus
                // Issue: click any point on popup trigger open keyboard/dropdown (default focus elements)
                iframe.contents().find(':focus').blur();
            }
        });
    };
    updatePopupTitleAfterLoad = (popUp, iframe) => {
        var title = popUp.find(".modal-title").html() || "";
        var iframeDocument = iframe.contents();

        var iframeTitle = iframeDocument.find("title").text();

        if (title == "" && iframeTitle != 'Home') {
            popUp.find(".modal-title").html(iframeTitle);
        }
    };
    copyTargetForm = (popUp) => {
        var oldTarget = this.topWindow.main.document.getElementById("target");
        var newTarget = this.topWindow.$.clone(oldTarget);

        popUp.find("iframe").remove();

        var iframeId = 'edit-setting-frame-' + Math.random().toString(36).substring(7);
        popUp.find('.modal-body').append('<iframe id="popup-iframe" frameborder="0" allowtransparency="yes" marginheight="0" marginwidth="0" scrolling="auto" name="' + iframeId + '"></iframe>');

        popUp.find('.modal-body').append(newTarget);
        newTarget.target = iframeId;
        newTarget.submit();
    };
    openPopUpAfterSetIframeSrc = (popUp, isAutoHeight, options) => {
        popUp.on('shown.bs.modal', function () {
            // transform property make objectFlash not show (MacauGames Result)
            popUp.find('.modal-dialog').css({
                transform: 'none',
                margin: 0,
                width: "100%"
            });

            // This will make popup isn't cut its height when its height larger than browser height
            if (isAutoHeight) {
                popUp.css({ display: 'table' });
            }
        }).on("hidden.bs.modal", () => {
            popUp.find("iframe").attr("src", "about:blank");
        });

        popUp.modal({ show: true, backdrop: options.backdrop });

        if (options.resizable) {
            var prevPosition = {};
            var prevSize = { width: 0, height: 0 };

            popUp.resizable({
                minHeight: 100,
                minWidth: 100,
                direction: ['right', 'bottom'],
                start: () => {
                    prevPosition = popUp.offset();
                    prevSize = this.getElementSize(popUp);
                },
                resize: () => {
                    var size = this.getElementSize(popUp);

                    popUp.find(".ex-modal").css("height", size.height + "px");

                    var headerHeight = 40;
                    var contentBorder = 2;
                    var bodyHeightPadding = 18;
                    popUp.find(".modal-content .modal-body").css("height", (size.height - headerHeight - contentBorder) + "px");
                },
                stop: () => {
                    var size = this.getElementSize(popUp);

                    if (prevSize.width == size.width && prevSize.height == size.height) {
                        return;
                    }

                    prevSize = size;
                    popUp.offset(prevPosition);
                }
            });
        }

        popUp.find(".icon-close").unbind("click").click(() => {
            this.activePopups.pop();
            popUp.find('.modal-body #target').remove();
            popUp.modal("hide");

            if (this.onClosing && typeof (this.onClosing) == "function") {
                this.onClosing();
            }
        });

        popUp.tinyDraggable({
            exclude: "a.icon-information-outline.icon-info, button.icon.icon-close.close",
            resizeElement: options.resizable ? ".resizable-handle" : ""
        });

        this.activePopups.push(popUp);
    };
    open = (options) => {
        this.setDefaultOptions(options);

        var widthHeight = this.calculateWidthHeight(options);
        var width = widthHeight.width;
        var height = widthHeight.height;

        var popupHeight = height;
        var popupWidth = width;

        var activePopup = this.topWindow.$("#" + options.handlerId);
        var popupId = options.handlerId;

        if (options.popupCss) {
            activePopup.addClass(options.popupCss);
        }

        this.popupId = popupId;

        // Calculate body scrolltop to fix issue: focus popup make body scroll to popup position (on device)
        var scrollTop = this.getScrollTop(window.document.body);
        var scrollLeft = this.getScrollLeft(window.document.body);

        // ON iOS: there is scroll issues with popup. We must show popup with auto height
        var isAutoHeight = this.isiOS();

        var left = options.centerScreen ? this.$(window).width() / 2 - popupWidth / 2 : options.left + scrollLeft;
        var popupCss = {
            position: "absolute",
            width: popupWidth,
            height: isAutoHeight ? 'auto' : popupHeight,
            left: left < 0 ? 0 : left,
            top: options.top + scrollTop,
            overflow: options.resizable ? "hidden" : undefined,
            "overflow-y": isAutoHeight ? "visible" : undefined,
            "overflow-x": isAutoHeight ? "auto" : undefined
        };

        activePopup.css(popupCss);

        var iframe = activePopup.find("iframe");
        iframe.attr("src", "about:blank");

        activePopup.find(".modal-title").text(options.title);

        if (!isAutoHeight) {
            activePopup.find(".ex-modal").css("height", popupHeight + "px");
        }

        var headerHeight = 40;
        var contentBorder = 2;
        var bodyHeightPadding = 18;
        activePopup.find(".modal-content .modal-body").css("height", isAutoHeight ? 'auto' : (popupHeight - headerHeight - contentBorder) + "px");

        var iframeHeight = popupHeight - headerHeight - contentBorder - bodyHeightPadding;
        this.setIframeInfo(iframe, activePopup, { height: iframeHeight, left: options.left, top: options.top });

        iframe.attr("src", options.url);

        if (options.url) {
            iframe.attr("src", options.url);
        } else {
            this.copyTargetForm(activePopup);
            iframe = activePopup.find("iframe");
            this.setIframeInfo(iframe, activePopup, { height: iframeHeight, left: options.left, top: options.top });
        }

        this.openPopUpAfterSetIframeSrc(activePopup, isAutoHeight, options);
    };
    bootboxDialog = (title: string, htmlContent: Function) => {
        let dialog = bootbox.dialog({
            title: title,
            message: "<p>Loading...</p>",
            buttons: {
                cancel: {
                    label: "Cancel",
                    className: "btn-cancel"
                },
                submit: {
                    label: "Submit",
                    className: "btn-submit",
                    callback: function () {
                        return false;
                    }
                }
            }
        });

        dialog.init(function () {
            dialog.find(".bootbox-body").html(htmlContent);
        });
    };
    openConfirm = (title: string, message: string) => {
        bootbox.confirm("Are you sure?", function (result) {
            /* your callback code */
        }).find(".modal-content").css({
            "top": "100px",
            "left": function () {
                let left = $(window).width() / 2 - $(".modal-dialog").width() / 2;

                return left + "px";
            }
        });
    };
    hasPopupOpen = (): boolean => this.activePopups.length > 0;
    revertLatestPopupTitle = (): void => {
        if (this.hasPopupOpen()) {
            let latestActivePopup: JQuery = this.$(this.activePopups[this.activePopups.length - 1]);
            let popupTitle: JQuery = latestActivePopup.find(".modal-title");

            if (popupTitle.length) {
                let previousTitle: string = popupTitle.attr("data-previous-title");
                let currentTitle: string = popupTitle.text();
                popupTitle.text(previousTitle !== currentTitle ? previousTitle : "");
            }
        }
    };
    setLatestPopupTitle = (title: string): void => {
        if (this.hasPopupOpen()) {
            let latestActivePopup: JQuery = this.$(this.activePopups[this.activePopups.length - 1]);
            let popupTitle: JQuery = latestActivePopup.find(".modal-title");

            if (popupTitle.length) {
                let previousTitle = popupTitle.text();
                popupTitle
                    .attr("data-previous-title", previousTitle)
                    .text(title);
            }
        }
    };
}

const popupManager = new PopupManager();