using ColossalFramework.Plugins;
using System.ComponentModel;
using UnityEngine;

namespace CitiesZombieMod
{
    public static class Logger
    {
        const string prefix = "ZombieMod: ";
        static bool debuggingEnabled = true;
        static bool logClassAndMethodName = true;

        public static void LogClassAndMethodName(string className, string methodName)
        {
            if (logClassAndMethodName)
            {
                Debug.Log("Class: " + className + " | Method: " + methodName);
            }
        }

        public static void Log(object s)
        {
            if (!debuggingEnabled) return;
            LogObject(PluginManager.MessageType.Message, s);
        }

        public static void Error(object s)
        {
            if (!debuggingEnabled) return;
            LogObject(PluginManager.MessageType.Error, s);
        }

        public static void Warning(object s)
        {
            if (!debuggingEnabled) return;
            LogObject(PluginManager.MessageType.Warning, s);
        }

        private static void LogObject(PluginManager.MessageType messageType, object myObject)
        {
            string myObjectDetails = "";
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myObject))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(myObject);
                myObjectDetails += name + ": " + value + "\n";
            }
            Debug.Log(messageType + " | " + prefix + " | " +myObjectDetails);
        }
    }
}
