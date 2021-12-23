import React from 'react';
import { Button, Alert } from 'antd';
import styles from "./styles.module.css";
import { alertTypes } from './types';

class Alerts extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            alertInfo: this.props.alertInfo
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {
        
        return {
            alertInfo: nextProps.alertInfo
        }
    }

    closeAlert = () => {

        const { 
            closeAlert 
        } = this.props;

        closeAlert();
    }

    showAlert = () => {

        switch (this.state.alertInfo.type) {
            case alertTypes.INFO: {
                return <div className={styles.alert}>
                    <Alert
                        message={this.state.alertInfo.message}
                        type="success"
                        showIcon
                        action={
                            <Button size="small" danger
                                onClick={() => this.closeAlert()}
                            >
                                Close
                            </Button>
                        }
                    />
                </div>
            }
            case alertTypes.WARNING: {
                return <div className={styles.alert}>
                    <Alert
                        message={this.state.alertInfo.message}
                        banner
                        action={
                            <Button size="small" danger
                                onClick={() => this.closeAlert()}
                            >
                                Close
                            </Button>
                        }
                    />
                </div>
            }
        }
    }

    render() {

        return (
            <>
                {this.showAlert()}
            </>
        );
    }
}

export default Alerts;