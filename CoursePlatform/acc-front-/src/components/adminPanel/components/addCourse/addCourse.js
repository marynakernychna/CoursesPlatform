import React from 'react';
import { Layout, Card, Col, Row, Button, Modal, Form, Input, Alert } from 'antd';
import './styles.css'
import coursesService from '../../../studentPanel/service';
import $ from 'jquery';
import EclipseWidget from '../../../eclipse'

const { Content } = Layout
const { TextArea } = Input;
const { Meta } = Card;

class AddCourse extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            warning: "",
            info: "",

            loading: this.props.loading
        };
    }

    componentDidMount() {
        $(".alert").css('visibility', 'hidden');
        $(".alert").css('maxHeight', '0px');

        $(".alertInfo").css('visibility', 'hidden');
        $(".alertInfo").css('maxHeight', '0px');
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {
        if (nextProps.warning != "" && nextProps.warning != undefined) {
            $(".alert").css('visibility', 'visible');
        }
        else if (nextProps.info != "" && nextProps.warning != undefined) {
            $(".alertInfo").css('visibility', 'visible');
        }

        return {
            info: nextProps.info,
            warning: nextProps.warning,
            loading: nextProps.loading
        }
    }

    // alerts actions

    closeWarningAlert = () => {
        $(".alert").css('visibility', 'hidden');

        const { clearWarning } = this.props;
        clearWarning();
    }

    closeInfoAlert = () => {
        $(".alertInfo").css('visibility', 'hidden');

        const { clearInfo } = this.props;
        clearInfo();
    }

    addCourse = (model) => {
        const {
            addCourseStarted,
            addCourseSuccess,
            addCourseFailed
        } = this.props;
        console.log(model);

        addCourseStarted();

        coursesService.addCourse(model)
            .then((response) => {
                addCourseSuccess();
            },
                err => {

                    addCourseFailed();
                })
            .catch(err => {
                console.log("Frontend error", err);
            });
    };

    render() {

        const { loading } = this.state;

        return (

            <Layout className="site-layout">

                {/* alerts */}

                <div className="alert" style={{ display: 'block', alignSelf: 'end' }}>
                    <Alert
                        message={this.state.warning}
                        banner
                        action={
                            <Button size="small" danger onClick={this.closeWarningAlert}>
                                Close
                            </Button>
                        }
                    />
                </div>
                <div className="alertInfo" style={{ display: 'block', alignSelf: 'end' }}>
                    <Alert
                        message={this.state.info}
                        type="success"
                        showIcon
                        action={
                            <Button size="small" danger onClick={this.closeInfoAlert}>
                                Close
                            </Button>
                        }
                    />
                </div>

                <Content
                    className="site-layout-background"
                    style={{
                        padding: 30,
                        minHeight: 280,
                    }}
                >
                    <Card hoverable
                        style={{ margin: '14px 0px' }}
                    >
                        <Form name="basic"
                            onFinish={this.addCourse}>

                            <Form.Item label="Enter course title :" name="title"
                                rules={[
                                    {
                                        required: true,
                                        message: 'Please enter course title!',
                                    },
                                ]}>
                                <TextArea allowClear
                                    showCount autoSize maxLength={70}
                                />
                            </Form.Item>

                            <Form.Item label="Enter course description :" name="description"
                                rules={[
                                    {
                                        required: true,
                                        message: 'Please enter course description!',
                                    },
                                ]}
                            >
                                <TextArea allowClear
                                    showCount autoSize maxLength={150}
                                />
                            </Form.Item>

                            <Form.Item label="Enter image url :" name="imageUrl"
                                rules={[
                                    {
                                        required: true,
                                        message: 'Please enter image url!',
                                    },
                                ]}
                            >
                                <TextArea allowClear
                                    showCount autoSize
                                />
                            </Form.Item>

                            <Form.Item>
                                <Button type="primary" htmlType="submit">Add</Button>
                            </Form.Item>
                        </Form>
                    </Card>
                </Content>

                {loading && <EclipseWidget />}

            </Layout >
        );
    }
}

export default AddCourse;