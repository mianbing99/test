/**
 * layui ��չtab���
 * Autor��master beginner / Fhua
 * Date : 16-11-26
 */

layui.define(['element', 'common'], function (exports) {
    var mod_name = 'tab',
		$ = layui.jquery,
		element = layui.element(),
		commom = layui.common,
		globalTabIdIndex = 0,
		Tab = function () {
		    this.config = {
		        elem: undefined,
		        closed: true, //�Ƿ����ɾ����ť
		        autoRefresh: true,//�Ƿ�ˢ��ifram
		        contextMenu: false//�Ƿ��Ҽ��˵�
		    };
		};
    var ELEM = {};
    /**
	 * ��������
	 * @param {Object} options
	 */
    Tab.prototype.set = function (options) {
        var that = this;
        $.extend(true, that.config, options);
        return that;
    };
    /**
	 * ��ʼ��
	 */
    Tab.prototype.init = function () {
        var that = this;
        var _config = that.config;
        if (typeof (_config.elem) !== 'string' && typeof (_config.elem) !== 'object') {
            common.throwError('Tab error: elem����δ��������ó����������ø�ʽ��ο��ĵ�API.');
        }
        var $container;
        if (typeof (_config.elem) === 'string') {
            $container = $('' + _config.elem + '');
        }
        if (typeof (_config.elem) === 'object') {
            $container = _config.elem;
        }
        if ($container.length === 0) {
            common.throwError('Tab error:�Ҳ���elem�������õ�����������.');
        }
        var filter = $container.attr('lay-filter');
        if (filter === undefined || filter === '') {
            common.throwError('Tab error:��Ϊelem��������һ��lay-filter������');
        }
        _config.elem = $container;
        ELEM.titleBox = $container.children('ul.layui-tab-title');
        ELEM.contentBox = $container.children('div.layui-tab-content');
        ELEM.tabFilter = filter;
        return that;
    };
    /**
	 * ��ѯtab�Ƿ���ڣ���������򷵻�����ֵ�������ڷ���-1
	 * @param {String} ����
	 */
    Tab.prototype.exists = function (title) {
        var that = ELEM.titleBox === undefined ? this.init() : this,
			tabIndex = -1;
        ELEM.titleBox.find('li').each(function (i, e) {
            var $cite = $(this).children('cite');
            if ($cite.text() === title) {
                tabIndex = i;
            };
        });
        return tabIndex;
    };
    /**
	 * ���ѡ�񿨣����ѡ�񿨴������ȡ����
	 * @param {Object} data
	 */
    Tab.prototype.tabAdd = function (data) {
        var that = this;
        var _config = that.config;
        var tabIndex = that.exists(data.title);
        if (tabIndex === -1) {
            globalTabIdIndex++;
            var content = '<iframe src="' + data.href + '" data-id="' + globalTabIdIndex + '" id="ifrid-' + globalTabIdIndex + '"></iframe>';
            var title = '';
            if (data.icon !== undefined) {
                if (data.icon.indexOf('fa-') !== -1) {
                    title += '<i class="' + data.icon + '" aria-hidden="true"></i>';
                } else {
                    title += '<i class="layui-icon">' + data.icon + '</i>';
                }
            }
            title += '<cite>' + data.title + '</cite>';
            if (that.config.closed) {
                title += '<i class="layui-icon layui-unselect layui-tab-close" data-id="' + globalTabIdIndex + '">&#x1006;</i>';
            }
            //���tab
            element.tabAdd(ELEM.tabFilter, {
                title: title,
                content: content
            });
            //iframe ����Ӧ
            ELEM.contentBox.find('iframe[data-id=' + globalTabIdIndex + ']').each(function () {
                $(this).height(ELEM.contentBox.height());
                //layer.msg('���ڼ��������Ժ�...', { icon: 5 });
                layer.load(2, {
                    shade: [0.01, '#fff'] //0.1͸���ȵİ�ɫ����
                });
                $(this).css("opacity", 0.0);
            });
            if (_config.closed) {
                //�����ر��¼�
                ELEM.titleBox.find('li').children('i.layui-tab-close[data-id=' + globalTabIdIndex + ']').on('click', function () {
                    element.tabDelete(ELEM.tabFilter, $(this).parent('li').index()).init();
                    if (_config.contextMenu) {
                        $(document).find('div.uiba-contextmenu').remove(); //�Ƴ��Ҽ��˵�dom

                    }
                });
            };
            //�رռ��ز�
            $('#ifrid-' + globalTabIdIndex).load(function () {
                var t = setTimeout("layer.closeAll();", 300);
                $(this).delay(600).animate({ opacity: '1' }, "slow");
            });
            //�л�����ǰ�򿪵�ѡ�
            element.tabChange(ELEM.tabFilter, ELEM.titleBox.find('li').length - 1);
        } else {
            element.tabChange(ELEM.tabFilter, tabIndex);
            //�Զ�ˢ��
            if (_config.autoRefresh) {
                _config.elem.find('div.layui-tab-content> div').eq(tabIndex).children('iframe')[0].contentWindow.location.reload();
            }
        }
        if (_config.contextMenu) {
            element.on('tab(' + ELEM.tabFilter + ')', function (data) {
                $(document).find('div.admin-contextmenu').remove();
            });
            ELEM.titleBox.find('li').on('contextmenu', function (e) {
                var $that = $(e.target);
                e.preventDefault();
                e.stopPropagation();

                var $target = e.target.nodeName === 'LI' ? e.target : e.target.parentElement;
                //�жϣ���������Ҽ��˵���div�����Ƴ�������ҳ����ֻ����һ��

                if ($(document).find('div.admin-contextmenu').length > 0) {
                    $(document).find('div.admin-contextmenu').remove();
                }
                //����һ��div

                var div = document.createElement('div');
                //����һЩ����

                div.className = 'admin-contextmenu';
                div.style.width = '120px';
                div.style.backgroundColor = 'white';

                var ul = '<ul>';
                ul += '<li data-target="refresh" title="ˢ�µ�ǰѡ�"><i class="fa fa-refresh" aria-hidden="true"></i> ˢ��</li>';
                ul += '<li data-target="closeCurrent" title="�رյ�ǰѡ�"><i class="fa fa-close" aria-hidden="true"></i> �رյ�ǰ</li>';
                ul += '<li data-target="closeOther" title="�ر�����ѡ�"><i class="fa fa-window-close-o" aria-hidden="true"></i> �ر�����</li>';
                ul += '<li data-target="closeAll" title="�ر�ȫ��ѡ�"><i class="fa fa-window-close-o" aria-hidden="true"></i> ȫ���ر�</li>';
                ul += '</ul>';
                div.innerHTML = ul;
                div.style.top = e.pageY + 2 + 'px';
                div.style.left = e.pageX + 'px';
                //��dom��ӵ�body��ĩβ

                document.getElementsByTagName('body')[0].appendChild(div);

                //��ȡ��ǰ���ѡ���idֵ
                var id = $($target).find('i.layui-tab-close').data('id');
                //��ȡ��ǰ���ѡ�������ֵ
                var clickIndex = $($target).index();
                var $context = $(document).find('div.admin-contextmenu');
                if ($context.length > 0) {
                    $context.eq(0).children('ul').children('li').each(function () {
                        var $that = $(this);
                        //�󶨲˵��ĵ���¼�

                        $that.on('click', function () {
                            //��ȡ�����targetֵ
                            var target = $that.data('target');
                            //

                            switch (target) {
                                case 'refresh': //ˢ�µ�ǰ

                                    var src = ELEM.contentBox.find('iframe[data-id=' + id + ']')[0].src;
                                    ELEM.contentBox.find('iframe[data-id=' + id + ']')[0].src = src;
                                    break;
                                case 'closeCurrent': //�رյ�ǰ

                                    if (clickIndex !== 0) {
                                        element.tabDelete(ELEM.tabFilter, clickIndex);
                                    }
                                    break;
                                case 'closeOther': //�ر�����

                                    ELEM.titleBox.children('li').each(function () {
                                        var $t = $(this);
                                        var id1 = $t.find('i.layui-tab-close').data('id');
                                        if (id1 != id && id1 !== undefined) {
                                            element.tabDelete(ELEM.tabFilter, $t.index());
                                        }
                                    });
                                    break;
                                case 'closeAll': //ȫ���ر�

                                    ELEM.titleBox.children('li').each(function () {
                                        var $t = $(this);
                                        if ($t.index() !== 0) {
                                            element.tabDelete(ELEM.tabFilter, $t.index());
                                        }
                                    });
                                    break;
                            }
                            //��������Ƴ��Ҽ��˵���dom

                            $context.remove();
                        });
                    });

                    $(document).on('click', function () {
                        $context.remove();
                    });
                }
                return false;
            });
        }
    };
    Tab.prototype.on = function (events, callback) {

    }

    var tab = new Tab();
    exports(mod_name, function (options) {
        return tab.set(options);
    });
});