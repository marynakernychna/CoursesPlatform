import React from 'react';
import coursesService from '../../../services/courses';
import { alertTypes } from '../../alert/types';
import Subscription from './subscription/index';

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

                setTotalCount(response.data.totalSize);
            },
                err => {
                    clearTotalCount();
                    this.setWarningAlert();
                })
            .catch(err => {
                clearTotalCount();
                console.log("Frontend error", err);
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
            elemOnPage: this.state.elementsOnPage,
            sortDirection: this.state.sortDirection,
            sortBy: this.state.sortBy
        }
    }

    render() {

        return (
            <div>
                {this.state.subscriptions.map((course, index) =>
                    <Subscription info={course} />
                )}
            </div>
        );
    }
}

export default Subscriptions;