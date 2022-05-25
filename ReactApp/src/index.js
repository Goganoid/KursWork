import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './views/Login/Login';
import Logout from './views/Logout/Logout';
import Registration from './views/Registration/Registration';
import Start from './views/Start/Start';
import Catalog from './views/catalog/Catalog';
import Tender from './views/Tender/Tender';
import ApiClient from './middleware/apiClient';
import Account from './views/Account/Account';
import Container from './components/container/Container';
import { Navigate } from 'react-router';
import AddTender from './views/add-tender/AddTender';
import ErrorPage from './views/ErrorPage/ErrorPage';
import Loading from './components/loading/Loading';
import { isLoggedIn } from './middleware/storage';
import AddCompany from './views/add-company/AddCompany';


const PrivateRoute = ({ children }) => {
    return isLoggedIn() ? children : <Navigate to="/login" />;
};

const apiClient = new ApiClient();
ReactDOM.render(<BrowserRouter>
    <Routes>
        <Route path="/" element={<App />}>
            <Route index element={<Start />} />
            <Route element={<Container />}>
                <Route path="catalog">
                    <Route index element={<Catalog apiClient={apiClient} />} />
                </Route>
                <Route path="tender">
                    <Route path=":tenderId" element={<Loading><Tender apiClient={apiClient} /></Loading>} />
                    <Route path="create"
                        element={<PrivateRoute><AddTender apiClient={apiClient} /></PrivateRoute>} />
                </Route>
                <Route path="company">
                    <Route path="create"
                        element={<PrivateRoute><AddCompany apiClient={apiClient} /></PrivateRoute>} />
                    <Route path="edit/:companyId"
                        element={<PrivateRoute><AddCompany apiClient={apiClient} /></PrivateRoute>} />
                </Route>
                <Route path="login" element={<Login apiClient={apiClient} />} />
                <Route path="register" element={<Registration apiClient={apiClient} />} />
                <Route path="logout" element={<Logout />} />
                <Route path="account/:companyId" element={<Loading><Account apiClient={apiClient} /></Loading>} />
                <Route path="account/" element={<Loading><Account apiClient={apiClient} /></Loading>} />
                <Route path="account/edit/:userId" element={<PrivateRoute><Registration apiClient={apiClient} /></PrivateRoute>} />
                <Route path="error/:errorCode" element={<ErrorPage></ErrorPage>} />
            </Route>
        </Route>
    </Routes>
</BrowserRouter>, document.getElementById('root'));