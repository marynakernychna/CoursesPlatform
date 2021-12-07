import React from 'react';
import { Button, Alert } from 'antd';
import styles from "./styles.module.css";

class Alerts extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            warning: "",
            info: "",

            isInfoAlert: false,
            isWarningAlert: false
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {
        
        if (nextProps.info != "" && nextProps.info != undefined) {
            return {
                info: nextProps.info,
                isInfoAlert: true,
                warning: "",
                isWarningAlert: false
            }
        }
        else if (nextProps.warning != "" && nextProps.warning != undefined) {
            return {
                warning: nextProps.warning,
                isWarningAlert: true,
                info: "",
                isInfoAlert: false
            }
        }
        else {
            return {
                info: "",
                warning: "",
                isInfoAlert: false,
                isWarningAlert: false
            }
        }
    }

    closeWarningAlert = () => {

        const { clearWarning } = this.props;
        clearWarning();
    }

    closeInfoAlert = () => {

        const { clearInfo } = this.props;
        clearInfo();
    }

    showAlerts = () => {

        if (this.state.isWarningAlert) {
            return <div className={styles.alert}>
                <Alert
                    message={this.state.warning}
                    banner
                    action={
                        <Button size="small" danger
                            onClick={this.closeWarningAlert}
                        >
                            Close
                        </Button>
                    }
                />
            </div>
        }
        else if (this.state.isInfoAlert) {
            return <div className={styles.alert}>
                <Alert
                    message={this.state.info}
                    type="success"
                    showIcon
                    action={
                        <Button size="small" danger
                            onClick={this.closeInfoAlert}
                        >
                            Close
                        </Button>
                    }
                />
            </div>
        }
    }

    render() {

        return (
            <>
                {this.showAlerts()}
            </>
        );
    }
}

export default Alerts;