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
            if (!logClassAndMethodName) return;
            LogText(PluginManager.MessageType.Message, "Class: " + className + " | Method: " + methodName);
        }

        public static void Log(string message)
        {
            if (!debuggingEnabled) return;
            LogText(PluginManager.MessageType.Message, message);
        }

        public static void Error(string Error)
        {
            if (!debuggingEnabled) return;
            LogText(PluginManager.MessageType.Error, Error);
        }

        public static void Warning(string warning)
        {
            if (!debuggingEnabled) return;
            LogText(PluginManager.MessageType.Warning, warning);
        }

        public static void LogObject(PluginManager.MessageType messageType, object myObject)
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

        private static void LogText(PluginManager.MessageType messageType, string text)
        {
            Debug.Log(messageType + " | " + prefix + " | " + text);
        }
    }
}
