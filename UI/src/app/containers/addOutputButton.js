import React, { Component } from 'react';

class AddOutputButton extends Component {

    render() {
        return(
            <div
            className="button btn btn-success item-button" 
            autoComplete="off"
            >
                <div><b>Add Output</b></div>
            </div>
        );
    }
}

export default AddOutputButton;