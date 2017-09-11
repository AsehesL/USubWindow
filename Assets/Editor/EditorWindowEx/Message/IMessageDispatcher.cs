using System;
using UnityEngine;
using System.Collections;

namespace EditorWinEx
{
    public interface IMessageDispatcher
    {

        Type GetDispatcherType();
    }
}
