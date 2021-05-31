import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { signOut } from '../OpenIdjs'

const LogoutButton = () => {
    const { logout } = useAuth0();

    return (
        <button onClick={() => signOut({ returnTo: window.location.origin })}>
            Log Out
    </button>
    );
};

export default LogoutButton;