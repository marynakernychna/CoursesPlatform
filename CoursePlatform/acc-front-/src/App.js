import React from 'react';
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import 'antd/dist/antd.css';
import Login from './components/auth/login/index';
import Registration from './components/auth/registration/index';
import StudentPanel from "./components/studentPanel/index"
import AdminPanel from "./components/adminPanel/index"
import Email from "./email/index"
import PrivateRoute from './PrivateRoute';
import { roles } from './actions/account/roles'

export default function App() {
  return (
    <Router>
      <Switch>
        <PrivateRoute path="/studentPanel" allowedRoles={[roles.STUDENT, roles.ADMIN]}>
          <StudentPanel />
        </PrivateRoute>
        <PrivateRoute path="/adminPanel" allowedRoles={[roles.ADMIN]}>
          <AdminPanel />
        </PrivateRoute>

        <Route path="/login" >
          <Login />
        </Route>
        <Route path="/registration" >
          <Registration />
        </Route>
        <Route path="/emailConfirmation" >
          <Email />
        </Route>
      </Switch>
    </Router>
  );
}