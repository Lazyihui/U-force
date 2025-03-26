using System;

namespace UF {

    public enum InputBindingSystemMode {
        ChooseAction,//选择动作
        WaitBindingInput,//等待输入绑定状态
        Error,//冲突或者输入无效
    }
}