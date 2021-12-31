import React from 'react';
import { Layout, Button, Space, Table, Input } from 'antd';
import { FormOutlined, CloseOutlined } from '@ant-design/icons';
import coursesService from '../../../../../services/courses';
import { modalsTypes } from '../../../../modal/modalsTypes';
import { alertTypes } from '../../../../alert/types';
import { elementsOnPage } from '../../../../../constants/elementsOnPageCount';
import { sortByTypes } from '../../../../../constants/sortByTypes';
import { sortDirectionTypes } from '../../../../../constants/sortDirectionTypes';
import moment from 'moment';

const { Content } = Layout;
const { Search } = Input;

class Courses extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            courses: [],
            dateFormat: "YYYY-MM-DD",

            elementsOnPage: elementsOnPage[10],
            sortBy: sortByTypes.DATE,
            sortDirection: sortDirectionTypes.ASC,
            isSortingChanged: this.props.isSortingChangedRedux,
            totalCount: 0,
            currentPage: 1,
            searchText: undefined
        };
    }

    componentDidMount() {

        document.title = "Courses";

        this.getCourses();
    }

    componentDidUpdate() {

        if (this.state.isSortingChanged) {

            this.getCourses();
        }
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        return {
            courses: nextProps.data,
            isSortingChangedRedux: nextProps.isSortingChanged
        }
    }

    getCourses() {

        const {
            startLoading,
            finishLoading,
            setCourses,
            clearTotalCount,
            resetIsSortChangedStatus,
            setTotalCount
        } = this.props;

        if (this.state.isSortingChanged) {

            resetIsSortChangedStatus();
        }

        startLoading();

        var model = {
            "searchText": this.state.searchText,
            "filterQuery": {
                "pageNumber": this.state.currentPage,
                "elementsOnPage": this.state.elementsOnPage,
                "sortDirection": this.state.sortDirection,
                "sortBy": this.state.sortBy
            }
        }
        
        coursesService.getCourses(model)
            .then((response) => {
                
                var courseKey = 0;

                response.data.courses.map((info, index) => {

                    info.key = courseKey;
                    info.createDate = moment(info.createDate).format(this.state.dateFormat);
                    courseKey = courseKey + 1;
                })

                this.setState({
                    totalCount: response.data.totalCount
                })

                setCourses(response.data.courses);
                setTotalCount(response.data.totalCount);

            },
                err => {
                    
                    clearTotalCount();
                    this.setWarningAlert();
                })
            .catch(err => {
                
                clearTotalCount();
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

    openModal = (record, type) => {
        const {
            openModal
        } = this.props;

        openModal({ type: type, info: record });
    }

    handlePageChange = (page, pageSize) => {

        if (pageSize != this.state.elementsOnPage) {

            this.setState({
                currentPage: 1,
                elementsOnPage: pageSize
            }, function () { this.getCourses() });
        }
        else {
            this.setState({
                currentPage: page,
                elementsOnPage: pageSize
            }, function () { this.getCourses() });
        }
    }

    onSearch = (value) => {

        if (value != this.state.searchText) {

            this.setState({
                searchText: value,
                currentPage: 1
            }, function () { this.getCourses() });
        }
    }

    sortBy = (pagination, filters, sorter) => {

        var order;
        var field;

        switch (sorter.order) {
            case "descend": {
                order = sortDirectionTypes.DESC;
                break;
            }
            case "ascend": {
                order = sortDirectionTypes.ASC;
                break;
            }
            default: {
                this.setState({
                    sortBy: sortByTypes.DATE,
                    sortDirection: sortDirectionTypes.ASC
                }, function () { this.getCourses() });
                return;
            }
        }

        switch (sorter.field) {
            case "title":
                {
                    field = sortByTypes.TITLE;
                    break;
                }
            case "createDate":
                {
                    field = sortByTypes.DATE;
                    break;
                }
        }

        this.setState({
            sortBy: field,
            sortDirection: order
        }, function () { this.getCourses() });
    }

    render() {

        const columns = [
            {
                key: '1',
                title: 'Title',
                dataIndex: 'title',
                sorter: true
            },
            {
                key: '2',
                title: 'Creation date',
                dataIndex: 'createDate',
                sorter: true
            },
            {
                title: 'Actions',
                dataIndex: '',
                key: 'x',
                render: (_, record) =>
                    <Space>
                        <Button type="primary" icon={<FormOutlined />}
                            className="edit-form-button"
                            style={{ backgroundColor: "orange" }}
                            onClick={() => this.openModal(record, modalsTypes.EDIT_COURSE)}>
                            Edit
                        </Button>
                        <Button type="primary" icon={<CloseOutlined />}
                            className="edit-form-button" danger
                            onClick={() => this.openModal(record, modalsTypes.REMOVE_COURSE)}>
                            Remove
                        </Button>
                    </Space>
            }
        ];

        const tableProps = {
            expandedRowRender: record => <p style={{ margin: 0 }}>{record.description}</p>
        };

        return (
            <>

                <Search
                    placeholder="input search text"
                    allowClear
                    enterButton="Search"
                    size="large"
                    onSearch={(value) => this.onSearch(value)}
                />

                <Table
                    {...tableProps}
                    columns={columns}
                    dataSource={this.state.courses}
                    showSorterTooltip={true}
                    pagination={{
                        position: "bottomRight",
                        current: this.state.currentPage,
                        total: this.state.totalCount,
                        onChange: (page, pageSize) => this.handlePageChange(page, pageSize)
                    }}
                    onChange={(pagination, filters, sorter) => this.sortBy(pagination, filters, sorter)}
                    style={{ "marginTop": "20px" }}
                />

            </>
        );
    }
}

export default Courses;