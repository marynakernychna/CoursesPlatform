import React, { Fragment,  } from 'react';
import service from './service'

class Email extends React.Component {

    onClick =()=> {
        var data = window.location.pathname.split('/');
        var tokenParts = data.slice(2, -1);

        var token = tokenParts.join('/');
        var email = data[data.length - 1];

        var model = {
            Token: token,
            Email: email
        };

        service.test(model)
            .then((response) => {
                this.props.history.push("/login");
            },
                err => {
                    console.log("Error: ", err.response);
                })
            .catch(err => {
                console.log("Global server error", err);
            });
    }

    render() {
        return (
            <Fragment>
                <button onClick={this.onClick}>Confirm</button>
            </Fragment >
        );
    }
}

export default Email;