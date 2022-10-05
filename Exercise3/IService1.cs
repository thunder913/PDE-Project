using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Exercise3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        [WebInvoke(UriTemplate = "Addit?num1={num1}&num2={num2}",
        Method = "GET",
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        ResponseFormat = WebMessageFormat.Xml
        )
        ]
        string AddIt(int num1, int num2);

        [OperationContract]
        [WebInvoke(UriTemplate = "getResource",
        Method = "GET",
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        ResponseFormat = WebMessageFormat.Json
        )
        ]
        string getResource();

        [OperationContract]
        [WebInvoke(UriTemplate = "addResource?id={id}&value={value}",
        Method = "POST",
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        ResponseFormat = WebMessageFormat.Json
        )
        ]
        string addResource(string id, string value);
        
        [OperationContract]
        [WebInvoke(UriTemplate =
        "updateResource?id={id}&value={value}&isdel={isdel}",
        Method = "POST",
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        ResponseFormat = WebMessageFormat.Json
        )
        ]
        string updateResource(string id, string value, bool isdel = false);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
