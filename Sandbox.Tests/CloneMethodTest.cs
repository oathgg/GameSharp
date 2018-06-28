using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Tests
{
    [TestClass]
    [Serializable]
    public class UtilityTests
    {
        [TestMethod]
        public void CloneMethodTest()
        {
            //UtilityTests t = Clone();
            
        }

        //public BitBlt Clone()
        //{
        //    MemoryStream ms = new MemoryStream();
        //    BinaryFormatter bf = new BinaryFormatter();

        //    bf.Serialize(ms, this);

        //    ms.Position = 0;
        //    object obj = bf.Deserialize(ms);
        //    ms.Close();

        //    return obj as UtilityTests;
        //}
    }
}
