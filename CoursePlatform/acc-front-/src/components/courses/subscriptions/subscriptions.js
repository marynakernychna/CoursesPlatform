import React from 'react';
import coursesService from '../../../services/courses';
import { alertTypes } from '../../alert/types';
import Subscription from './subscription/index';
import PagePagination from '../../pagination/index';
import SortMenu from '../../sortMenu/index';
import { Layout, Result } from 'antd';

const { Content } = Layout;

class Subscriptions extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            subscriptions: [],

            currentPage: this.props.currentPage,
            elementsOnPage: this.props.elementsOnPage,
            sortDirection: this.props.sortDirection,
            sortBy: this.props.sortBy,

            isSortingChanged: this.props.isSortingChanged,
            isElementsChanged: this.props.isElementsChanged
        };
    }

    componentDidMount() {

        document.title = "Subscriptions";

        var model = this.formRequestModel();

        this.getCourses(model);
    }

    componentDidUpdate() {

        if (this.state.isElementsChanged || this.state.isSortingChanged) {

            const {
                resetIsSortChangedStatus,
                resetIsElementsChanged
            } = this.props;

            if (this.state.isElementsChanged) {
                resetIsElementsChanged();
            }
            else {
                resetIsSortChangedStatus();
            }

            var model = this.formRequestModel();
            this.getCourses(model);
        }
    }

    static getDerivedStateFromProps = (nextProps, prevState) => {

        if (nextProps.isSortingChanged || nextProps.isElementsChanged) {

            return {
                elementsOnPage: nextProps.elementsOnPage,
                sortDirection: nextProps.sortDirection,
                sortBy: nextProps.sortBy,
                currentPage: nextProps.currentPage,

                isSortingChanged: nextProps.isSortingChanged,
                isElementsChanged: nextProps.isElementsChanged
            }
        }

        return {
            isSortingChanged: nextProps.isSortingChanged,
            isElementsChanged: nextProps.isElementsChanged
        }
    }

    getCourses = (model) => {

        const {
            startLoading,
            finishLoading,
            setTotalCount,
            clearTotalCount
        } = this.props;

        startLoading();

        coursesService.getSubscriptions(model)
            .then((response) => {

                this.setState({
                    subscriptions: response.data.subscriptions
                });

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

    formRequestModel = () => {
        return {
            pageNumber: this.state.currentPage,
            elementsOnPage: this.state.elementsOnPage,
            sortDirection: this.state.sortDirection,
            sortBy: this.state.sortBy
        }
    }

    render() {

        return (

            <Content
                style={{
                    padding: 30,
                    minHeight: 280
                }}
            >
                {this.state.subscriptions.length != 0 ?
                    <>
                        <SortMenu />

                        <div>
                            {this.state.subscriptions.map((course, index) =>
                                <Subscription info={course} />
                            )}
                        </div>

                        <PagePagination />

                    </>
                    :
                    <Result
                        title="You have not yet enrolled in any course"
                    />
                }
            </Content>
        );
    }
}

export default Subscriptions;