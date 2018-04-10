import React, { Component } from 'react';

class AddFixtureButton extends Component {

    render() {
        return(
            <div
            className="button btn btn-success item-button" 
            autoComplete="off"
            >
                <div><b>Add Fixture</b></div>
            </div>
        );
    }
}

export default AddFixtureButton;