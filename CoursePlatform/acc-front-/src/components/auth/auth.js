import React from 'react';
import styles from './styles.module.css'
import Alerts from '../alert/index';
import { pagesNames } from '../../constants/pagesNames';
import Login from './login/index';
import Registration from './registration/index';
import { Spin } from 'antd';

class AuthPage extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            isAlert: this.props.isAlert,
            loading: this.props.loading,
            pageName: this.props.type
        };
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            isAlert: nextProps.isAlert,
            loading: nextProps.loading,
            pageName: nextProps.pageName
        }
    }

    showContent = () => {

        switch (this.state.pageName) {
            case pagesNames.LOGIN: {
                return <Login history={this.props.history} />
            }
            case pagesNames.REGISTRATION: {
                return <Registration />
            }
        }
    }

    render() {

        const { loading, isAlert } = this.state;

        return (

            <>

                <div className={styles.contentBlock}>

                    <Spin size="large" spinning={loading}>
                        {this.showContent()}

                    </Spin>
                    
                </div>

                {isAlert && <Alerts />}

            </>

        )
    };
}

export default AuthPage;
