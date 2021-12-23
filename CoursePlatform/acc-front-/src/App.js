import React from 'react';
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import 'antd/dist/antd.css';
import Email from "./components/email/index";
import PrivateRoute from './PrivateRoute';
import AuthPage from './components/auth/index';
import { roles } from './constants/roles';
import { pagesNames } from './constants/pagesNames';
import CoursesPage from './components/courses/index';
import Subscriptions from './components/courses/subscriptions/index';
import AllCourses from './components/courses/allCourses/index';
import Home from './components/home/home';
import Courses from './components/adminPanelPage/content/dataTable/courses/index';
import AdminPanelPage from './components/adminPanelPage/index';
import AddCourse from './components/adminPanelPage/content/addCourse/index';
import Students from './components/adminPanelPage/content/dataTable/students/index';
import Profile from './components/profile/index';

const StudentPageLayoutRoute = ({ component: Component, ...rest }) => {
  return (
    <PrivateRoute
      {...rest}
    >
      <Route
      render={matchProps => (
        <CoursesPage>
          <Component {...matchProps} />
        </CoursesPage>
      )}
      />
    </PrivateRoute>
  );
};

const AdminPageLayoutRoute = ({ component: Component, ...rest }) => {
  return (
    <PrivateRoute
      {...rest}
    >
      <Route
      render={matchProps => (
        <AdminPanelPage>
          <Component {...matchProps} />
        </AdminPanelPage>
      )}
      />
    </PrivateRoute>
  );
};

export default function App() {
  return (
    <Router>
      <Switch>

        <StudentPageLayoutRoute exact path="/student/subscriptions" allowedRoles={[roles.STUDENT]} component={Subscriptions} />
        <StudentPageLayoutRoute exact path="/student/courses" allowedRoles={[roles.STUDENT]} component={AllCourses} />
        <StudentPageLayoutRoute exact path="/student/profile" allowedRoles={[roles.STUDENT]} component={Profile} />

        <AdminPageLayoutRoute exact path="/admin/courses" allowedRoles={[roles.ADMINISTRATOR]} component={Courses} />
        <AdminPageLayoutRoute exact path="/admin/add-course" allowedRoles={[roles.ADMINISTRATOR]} component={AddCourse} />
        <AdminPageLayoutRoute exact path="/admin/students" allowedRoles={[roles.ADMINISTRATOR]} component={Students} />

        <Route path="/auth" >
          <AuthPage type={pagesNames.LOGIN} />
        </Route>
        <Route path="/emailConfirmation" >
          <Email />
        </Route>

        <Route path="/" >
          <Home />
        </Route>

      </Switch>
    </Router>
  );
}