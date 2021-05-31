import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import { Auth0Provider } from '@auth0/auth0-react';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
    <BrowserRouter basename={baseUrl}>
        <div>
        <Auth0Provider
                domain="localhost:44386"
                clientId="react"
                redirectUri={window.location.origin}
        >
            <App />
            </Auth0Provider>
        </div>
  </BrowserRouter>,

  rootElement);

registerServiceWorker();

