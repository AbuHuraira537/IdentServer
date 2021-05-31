import React from "react";



import { UserManager } from 'oidc-client';
import axios from 'axios';
var config = {
   
    authority: "https://localhost:44386/",
    client_id: "react",
    redirect_uri: "https://localhost:44380",
    response_type: "code",
    scope: "openid",
}

var userManager = new UserManager(config);





const LoginButton = () => {
   // const { loginWithRedirect, loginWithPopup } = useAuth0();

    const signIn = () => {

        userManager.signinRedirect();
    }


    return <button onClick={() => signIn()}>Log In</button>;
};

export default LoginButton;