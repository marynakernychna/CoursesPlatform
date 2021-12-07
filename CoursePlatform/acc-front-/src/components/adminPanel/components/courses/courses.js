import React from 'react';
import { Layout, Card, Button, Modal, Form, Input, Alert, Pagination, Menu, Dropdown, Image } from 'antd';
import { FormOutlined, CloseOutlined } from '@ant-design/icons';
import { Space } from 'antd';
import moment from 'moment';
import coursesService from '../../../studentPanel/service';
import $ from 'jquery';
import EclipseWidget from '../../../eclipse'

const { Content } = Layout
const { Meta } = Card;
const { TextArea } = Input;

class Courses extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            courses: [],

            currentPage: 1,
            totalCoursesCount: 1,
            elementsOnPage: 10,
            sortDirection: 0,
            sortBy: 1,

            isEditModal: false,
            isRemoveModal: false,

            courseForEdit: {},
            newUrl: "",

            warning: "",
            info: "",

            dateFormat: "YYYY-MM-DD",

            loading: this.props.loading
        };
    }

    componentDidMount() {
        $(".alert").css('visibility', 'hidden');
        $(".alert").css('maxHeight', '0px');

        $(".alertInfo").css('visibility', 'hidden');
        $(".alertInfo").css('maxHeight', '0px');

        var model = {
            PageNumber: this.state.currentPage,
            ElemOnPage: this.state.elementsOnPage,
            SortDirection: this.state.sortDirection,
            SortBy: this.state.sortBy
        }

        this.getCourses(model);
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

    // courses actions

    getCourses(model) {

        const {
            getCoursesStarted,
            getCoursesSuccess,
            getCoursesFailed
        } = this.props;

        getCoursesStarted();

        coursesService.getCoursesList(model)
            .then((response) => {

                this.setState({
                    currentPage: model.PageNumber,
                    totalCoursesCount: response.data.totalSize,
                    courses: response.data.courses
                });

                getCoursesSuccess();
            },
                err => {
                    getCoursesFailed();
                })
            .catch(err => {
                console.log("Frontend error", err);
            });
    }

    showCourses = () => {
        return <div>
            {this.state.courses.map((info, index) =>
                <div style={{ width: '100%' }} >
                    <Card
                        hoverable
                        style={{ margin: '14px 0px' }}
                    >
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <img src={info.imageUrl}
                                            alt="Image not found"
                                            style={{ maxWidth: '150px', maxHeight: '150px' }}
                                        />
                                    </td>
                                    <td style={{ width: '100%', paddingLeft: "20px" }}>
                                        <h2> {info.title}</h2>
                                        <h4> {info.description} </h4>
                                        <br />

                                        <Meta description={moment(info.createDate).format(this.state.dateFormat)} />

                                        <Button type="primary" icon={<FormOutlined />}
                                            className="edit-form-button"
                                            style={{ marginTop: '10px', width: '100%',  backgroundColor : "orange" }}
                                            onClick={() => this.openEditModal(info)}>
                                            Edit
                                        </Button>
                                        <Button type="primary" icon={<CloseOutlined />}
                                            style={{ marginTop: '10px', width: '100%' }}
                                            className="edit-form-button" danger
                                            onClick={() => this.openRemoveModal(info)}>
                                            Remove
                                        </Button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </Card>
                </div>
            )}

            <div style={{ textAlign: 'center' }}>
                <Pagination
                    current={this.state.currentPage}
                    pageSize={this.state.elementsOnPage}
                    total={this.state.totalCoursesCount}
                    showTotal={(total, range) => `${range[0]}-${range[1]} of ${total} items`}
                    onChange={this.changeCurrentPage}
                    style={{ paddingTop: '20px' }} />
            </div>
        </div>
    }

    // modals actions

    openEditModal = (course) => {
        
        this.setState({
            isEditModal: true,
            courseForEdit: course,
            newUrl: course.imageUrl
        });
    }
    
    onImageUrlChange = () => {
        var newUrl = $(".imageUrl").val();

        this.setState({
            newUrl: newUrl
        });
    }

    openRemoveModal(course) {
        const {
            removeCourseStarted,
            removeCourseSuccess,
            removeCourseFailed
        } = this.props;

        this.setState({
            isRemoveModal: true,
            courseForEdit: course
        });
    }

    closeModal = () => {

        this.setState({
            isEditModal: false,
            isRemoveModal: false,
            courseForEdit: {}
        })
    };

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

    // pagination actions

    changeCurrentPage = (page) => {

        var model = {
            PageNumber: page,
            ElemOnPage: this.state.elementsOnPage,
            SortDirection: this.state.sortDirection,
            SortBy: this.state.sortBy
        }

        this.setState({
            currentPage: page
        });

        this.getCourses(model);
    };

    // sort actions

    changeElementsOnPageCount = (onPage) => {

        var model = {
            PageNumber: 1,
            ElemOnPage: onPage,
            SortDirection: this.state.sortDirection,
            SortBy: this.state.sortBy
        }

        this.getCourses(model);

        this.setState({
            elementsOnPage: onPage,
            currentPage: 1
        });
    }

    changeElementsDirectionSort = (direction) => {

        var model = {
            PageNumber: this.state.currentPage,
            ElemOnPage: this.state.elementsOnPage,
            SortDirection: direction,
            SortBy: this.state.sortBy
        }

        this.getCourses(model);

        this.setState({
            sortDirection: direction
        });
    }

    changeElementsSortBy = (sortBy) => {

        var model = {
            PageNumber: this.state.currentPage,
            ElemOnPage: this.state.elementsOnPage,
            SortDirection: this.state.sortDirection,
            SortBy: sortBy
        }

        this.getCourses(model);

        this.setState({
            sortBy: sortBy
        });
    }

    // main actions

    removeCourse = (courseId) => {

        var model = {
            Value: courseId
        }

        this.props.removeCourseStarted();

        this.closeModal();

        coursesService.removeCourse(model)
            .then(() => {

                var model = {
                    PageNumber: this.state.currentPage,
                    ElemOnPage: this.state.elementsOnPage,
                    SortDirection: this.state.sortDirection,
                    SortBy: this.state.sortBy
                }

                this.getCourses(model);

                this.props.removeCourseSuccess();
            },
                err => {
                    this.props.removeCourseFailed();
                })
            .catch(err => {
                console.log("Frontend error", err);
            });
    }

    editCourse = (values) => {
        const {
            editCourseStarted,
            editCourseSuccess,
            editCourseFailed
        } = this.props;
        
        if (values.title == undefined &&
            values.description == undefined &&
            values.imageUrl == undefined) {
                
                editCourseSuccess();
                this.closeModal();

                return;
            }

            if (values.title == undefined) {
                values.title = this.state.courseForEdit.title;
        }
        if (values.description == undefined) {
            values.description = this.state.courseForEdit.description;
        }
        if (values.imageUrl == undefined) {
            values.imageUrl = this.state.courseForEdit.imageUrl;
        }
        
        var model = {
            Id: this.state.courseForEdit.id,
            Title: values.title,
            Description: values.description,
            ImageUrl: values.imageUrl
        }

        editCourseStarted();

        this.closeModal();

        coursesService.editCourse(model)
            .then(() => {

                var model = {
                    PageNumber: this.state.currentPage,
                    ElemOnPage: this.state.elementsOnPage,
                    SortDirection: this.state.sortDirection,
                    SortBy: this.state.sortBy
                }

                editCourseSuccess();

                this.getCourses(model);
            },
                err => {
                    editCourseFailed();
                })
            .catch(err => {
                console.log("Frontend error", err);
            });
    };

    render() {

        // for modal 

        const layout = {
            labelCol: {
                span: 8,
            },
            wrapperCol: {
                span: 16,
            },
        };

        const tailLayout = {
            wrapperCol: {
                offset: 8,
                span: 16,
            },
        };

        // for menu

        const elementsOnPageMenu = (
            <Menu>
                <Menu.Item>
                    <a onClick={() => this.changeElementsOnPageCount(5)}>
                        5
                    </a>
                </Menu.Item>
                <Menu.Item>
                    <a onClick={() => this.changeElementsOnPageCount(10)}>
                        10
                    </a>
                </Menu.Item>
                <Menu.Item>
                    <a onClick={() => this.changeElementsOnPageCount(20)}>
                        20
                    </a>
                </Menu.Item>
            </Menu>
        );

        const directionMenu = (
            <Menu>
                <Menu.Item>
                    <a onClick={() => this.changeElementsDirectionSort(0)}>
                        by ascending
                    </a>
                </Menu.Item>
                <Menu.Item>
                    <a onClick={() => this.changeElementsDirectionSort(1)}>
                        by descending
                    </a>
                </Menu.Item>
            </Menu>
        );

        const sortByMenu = (
            <Menu>
                <Menu.Item>
                    <a onClick={() => this.changeElementsSortBy(1)}>
                        title
                    </a>
                </Menu.Item>
                <Menu.Item>
                    <a onClick={() => this.changeElementsSortBy(0)}>
                        creation date
                    </a>
                </Menu.Item>
            </Menu>
        );

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
                    {/* Menu */}

                    <Space direction="vertical">
                        <Space wrap>
                            <Dropdown overlay={elementsOnPageMenu} placement="bottomCenter">
                                <Button>Elements on page</Button>
                            </Dropdown>
                        </Space>
                    </Space>

                    <Space direction="vertical" style={{ marginLeft: '10px' }}>
                        <Space wrap>
                            <Dropdown overlay={directionMenu} placement="bottomCenter">
                                <Button>Sort direction</Button>
                            </Dropdown>
                        </Space>
                    </Space>

                    <Space direction="vertical" style={{ marginLeft: '10px' }}>
                        <Space wrap>
                            <Dropdown overlay={sortByMenu} placement="bottomCenter">
                                <Button>Sort by</Button>
                            </Dropdown>
                        </Space>
                    </Space>

                    {this.showCourses()}

                </Content>

                <Modal title="Edit the course"
                    visible={this.state.isEditModal}
                    onCancel={this.closeModal}
                    afterClose={this.closeModal}
                    okButtonProps={{ style: { display: 'none' } }}
                    destroyOnClose={"true"}>

                    <Form {...layout} name="basic"
                        onFinish={this.editCourse}
                        style={{ "textAlign": 'right' }}>

                        <Form.Item label="Enter new title :" name="title">
                            <TextArea className="title" allowClear
                                showCount autoSize maxLength={70}
                                defaultValue={this.state.courseForEdit.title}
                                value={this.state.courseForEdit.title}
                                />
                        </Form.Item>

                        <Form.Item label="Enter new description :" name="description">
                            <TextArea className="description" allowClear
                                showCount autoSize maxLength={150} minLength={4}
                                defaultValue={this.state.courseForEdit.description} 
                                value={this.state.courseForEdit.description}
                                />
                        </Form.Item>

                        <Form.Item label="Enter new image url :" name="imageUrl">
                            <TextArea className="imageUrl" onChange={this.onImageUrlChange}
                                autoSize allowClear
                                defaultValue={this.state.courseForEdit.imageUrl} 
                                value={this.state.courseForEdit.imageUrl}
                                />
                        </Form.Item>

                        <Image width={200}
                            src={this.state.newUrl}
                        />

                        <Form.Item {...tailLayout}
                            style={{ "marginTop": '10px' }}>
                            <Button type="primary" htmlType="submit">Submit</Button>
                        </Form.Item>
                    </Form>
                </Modal>

                <Modal title="Confirm removing"
                    visible={this.state.isRemoveModal}
                    onCancel={this.closeModal}
                    afterClose={this.closeModal}
                    onOk={() => this.removeCourse(this.state.courseForEdit.id)}>
                    <Form {...layout} name="basic"
                        style={{ "textAlign": 'right' }}>

                        <h4>Delete "{this.state.courseForEdit.title}" course?</h4>

                    </Form>
                </Modal>

                {loading && <EclipseWidget />}

            </Layout>
        );
    }
}

export default Courses;


// class EditModal extends React.Component {

//     constructor(props) {
//         super(props);
//         this.state = {
//             title: ""
//         };
//     }

//     render () {
//         // for modal 

//         const layout = {
//             labelCol: {
//                 span: 8,
//             },
//             wrapperCol: {
//                 span: 16,
//             },
//         };

//         const tailLayout = {
//             wrapperCol: {
//                 offset: 8,
//                 span: 16,
//             },
//         };
//         return (
//             <Modal title="Edit the course"
//                     visible={this.props.visible}
//                     onCancel={() => this.props.closeModal()}
//                     afterClose={() => this.props.closeModal()}
//                     okButtonProps={{ style: { display: 'none' } }}>
//                     <Form {...layout} name="basic"
//                         onFinish={() => this.props.editCourse()}
//                         style={{ "textAlign": 'right' }}>

//                         <Form.Item label="Enter new title :" name="title">
//                             <TextArea className="title" allowClear
//                                 showCount autoSize maxLength={50}
//                                 defaultValue={this.props.title}
//                                 />
//                         </Form.Item>

//                         <Form.Item label="Enter new description :" name="description">
//                             <TextArea className="description" allowClear
//                                 showCount autoSize maxLength={150} minLength={20}
//                                 defaultValue={this.state.courseForEdit.description} />
//                         </Form.Item>

//                         <Form.Item label="Enter new image url :" name="imageUrl">
//                             <TextArea className="imageUrl" onChange={this.onImageUrlChange}
//                                 autoSize allowClear
//                                 defaultValue={this.state.courseForEdit.imageUrl} />
//                         </Form.Item>

//                         <Image width={200}
//                             src={this.state.newUrl}
//                         />

//                         <Form.Item {...tailLayout}
//                             style={{ "marginTop": '10px' }}>
//                             <Button type="primary" htmlType="submit">Submit</Button>
//                         </Form.Item>
//                     </Form>
//                 </Modal>
//     );
// }
// }