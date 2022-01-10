import React from 'react';
import { Card, Button } from 'antd';
import usersService from './../../services/students';
import { alertTypes } from './../alert/types';
import moment from 'moment';
import { FormOutlined } from '@ant-design/icons';
import { modalsTypes } from '../modal/modalsTypes';
import { loginViaFacebook } from './../../constants/others';

const { Meta } = Card;

class Profile extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            profileInfo: {},
            dateFormat: "YYYY-MM-DD",
            isWithoutBirthday: false,
            birthday: "1/1/1",
            age: -1
        }
    }

    componentDidMount() {
        document.title = "Profile";
        this.loadProfile();
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        if (nextProps.profileInfo != undefined &&
            nextProps.profileInfo.name != undefined &&
            nextProps.profileInfo != prevState.profileInfo) {

            return {
                profileInfo: nextProps.profileInfo
            }
        }

        if (nextProps.date != undefined) {

            var date = moment(nextProps.date.date).format(prevState.dateFormat);

            return {
                birthday: date,
                age: parseInt(moment.duration(moment().diff(new Date(date))).asYears(), 10)
            }
        }
    }

    loadProfile = () => {

        const {
            startLoading,
            finishLoading
        } = this.props;

        startLoading();

        usersService.getProfileInfo()
            .then((response) => {

                response.data.age = parseInt(moment.duration(moment().diff(new Date(response.data.birthday))).asYears(), 10);

                if (response.data.age < 14) {

                    response.data.age = loginViaFacebook;
                    response.data.birthday = loginViaFacebook;

                    this.setState({
                        isWithoutBirthday: true
                    });
                }
                else {
                    response.data.birthday = moment(response.data.birthday).format(this.state.dateFormat)
                }

                this.setState({
                    profileInfo: response.data,
                    birthday: response.data.birthday,
                    age: response.data.age
                });
            },
                err => {

                    this.setWarningAlert();
                })
            .catch(err => {

                this.setWarningAlert();
            })
            .finally(() => {
                finishLoading();
            });
    }

    setWarningAlert = () => {

        const {
            setAlert
        } = this.props;

        var model = {
            type: alertTypes.WARNING,
            message: "Something went wrong, try again !"
        }

        setAlert(model);
    }

    // modals actions

    openModal = (info, type) => {

        const {
            openModal
        } = this.props;

        openModal({ type: type, info: info });
    }

    render() {

        const { isWithoutBirthday } = this.state;

        return (

            <div style={{
                width: '100%'
            }} >

                <Card
                    hoverable
                    style={{
                        margin: '45px 15px',
                        padding: "0px",
                        borderRadius: "8px"
                    }}
                >

                    <h2 style={{ "fontWeight": "400" }}>Profile Details</h2>

                    <hr style={{ "margin": "25px 0" }} />

                    <table>
                        <tr>
                            <td style={{ "color": "gray", "fontWeight": "550" }}>
                                <p>Name</p>
                            </td>
                            <td>
                                <p>{this.state.profileInfo.name}</p>
                            </td>
                        </tr>
                        <tr>
                            <td style={{ "color": "gray", "fontWeight": "550" }}>
                                <p>Surname</p>
                            </td>
                            <td>
                                <p>{this.state.profileInfo.surname}</p>
                            </td>
                        </tr>
                        <tr>
                            <td style={{ "color": "gray", "fontWeight": "550" }}>
                                <p>Email</p>
                            </td>
                            <td>
                                <p>{this.state.profileInfo.email}</p>
                            </td>
                        </tr>
                        <tr>
                            <td style={{ "color": "gray", "fontWeight": "550", "paddingRight": "350px" }}>
                                <p>Birthday</p>
                            </td>
                            <td>
                                <p>{this.state.birthday}</p>
                            </td>
                        </tr>
                        <tr>
                            <td style={{ "color": "gray", "fontWeight": "550" }}>
                                <p>Age</p>
                            </td>
                            <td>
                                <p>{this.state.age}</p>
                            </td>
                        </tr>
                    </table>

                    <Button type="primary" icon={<FormOutlined />}
                        className="edit-form-button"
                        style={{ backgroundColor: "orange", "marginTop": "20px" }}
                        onClick={() => this.openModal(this.state.profileInfo, modalsTypes.EDIT_PROFILE)}
                    >
                        Change
                    </Button>

                    <br />

                    {!isWithoutBirthday ?
                        <Button type="primary" icon={<FormOutlined />}
                            className="edit-form-button"
                            style={{ backgroundColor: "orange", "marginTop": "20px" }}
                            onClick={() => this.openModal(this.state.profileInfo, modalsTypes.CHANGE_PASSWORD)}
                        >
                            Change password
                        </Button> : <></>}

                </Card>

            </div>
        )
    };
}

export default Profile;