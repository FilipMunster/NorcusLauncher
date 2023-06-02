# NorcusLauncher
appid={D00485DE-5C61-4B74-BEB1-B98B9F219E0E}

API:
        public interface IClient
        {
            int Id { get; set; }
            string Name { get; set; }
            string Display { get; set; }
            bool IsRunning { get; set; }
        }

[RestResource(BasePath ="api/v1/clients")]

        [RestRoute("Get", "")]
        [RestRoute("Get", "{id:num}")]

new Claim("CanControlClients", "true")
        [RestRoute("Post", "run")]
        [RestRoute("Post", "{id:num}/run")]
        [RestRoute("Post", "stop")]
        [RestRoute("Post", "{id:num}/stop")]
        [RestRoute("Post", "restart")]
        [RestRoute("Post", "{id:num}/restart")]
        [RestRoute("Post", "identify")]
        [RestRoute("Post", "{id:num}/identify")]

new Claim("CanControlMachine", "true")
        [RestRoute("Post", "shutdown")]
        [RestRoute("Post", "restart")]

