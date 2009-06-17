namespace HopSharp.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class HoptoadTests
    {
        [Test]
        public void Can_send_an_exception()
        {
            try
            {
                throw new NotImplementedException("Booo");
            }
            catch (Exception e)
            {
                // TODO would like to mock the HttpWebRequest call... maybe dig up my TypeMock license
                var a = new HoptoadClient(e);
                a.Send();
            }
        }

        [Test]
        public void Can_convert_HoptoadNotice_to_json()
        {
            var notice = new HoptoadNotice
                             {
                                 ApiKey = "12345678",
                                 ErrorMessage = "sdlfds",
                                 ErrorClass = "sdflshs",
                                 Backtrace = "blah1\npoop2"
                             };

            var json = notice.Serialize();

            Console.WriteLine(json);
            Assert.AreEqual("{\"notice\":{\"api_key\":\"12345678\",\"error_class\":\"sdflshs\",\"error_message\":\"sdlfds\",\"environment\":{\"RAILS_ENV\":\"Default [Debug]\"},\"request\":null,\"session\":null,\"backtrace\":[\"blah1\",\"poop2\"]}}", json);
        }
    }
}