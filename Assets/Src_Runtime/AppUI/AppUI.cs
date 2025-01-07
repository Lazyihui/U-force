using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public class AppUI {

        public UIContext ctx;
        public UIEventCenter eventCenter;

        public AppUI() {
            ctx = new UIContext();
            eventCenter = new UIEventCenter();
        }

        public void Inject(Canvas screenCanvas, AssetsCore assetsCore) {
            ctx.Inject(screenCanvas, assetsCore);
        }

        public void Panel_Login_Open() {
            Panel_Login panel = ctx.panel_Login;

            if (panel == null) {

                GameObject prefab = ctx.assetsCore.Panel_GetLogin();

                GameObject go = GameObject.Instantiate(prefab, ctx.screenCanvas.transform);
                panel = go.GetComponent<Panel_Login>();

                panel.Ctor();

                panel.OnLoginClickHandle += () => {
                    eventCenter.OnLoginStart();
                };

            }
            ctx.panel_Login = panel;
            panel.Show();
        }

        public void Panel_Login_Close() {
            Panel_Login panel = ctx.panel_Login;
            if (panel == null) {
                return;
            }

            panel.TearDown();
            ctx.panel_Login = null;
        }

    }
}