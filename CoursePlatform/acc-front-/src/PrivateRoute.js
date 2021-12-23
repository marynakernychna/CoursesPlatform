import React from "react";
import { connect } from "react-redux";
import { Redirect, Route } from "react-router"
import { withRouter } from "react-router-dom";

const PrivateRoute = props => {
    const { isAuthUser, role, allowedRoles } = props;

    if (isAuthUser && allowedRoles.includes(role)) return props.children;
    if (isAuthUser && !allowedRoles.includes(role)) return <Route component={() => <Redirect to="/auth" />} />;
    if (!isAuthUser) return <Route component={() => <Redirect to="/auth" />} />;

    return <Route {...props} />;
};

const mapStateToProps = (stateRedux) => ({
    isAuthUser: stateRedux.generalReducer.isAuthUser,
    role: stateRedux.generalReducer.role
});

export default withRouter(connect(mapStateToProps)(PrivateRoute));