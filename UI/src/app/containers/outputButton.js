import React, { Component } from 'react';

class OutputButton extends Component {

    render() {
        return(
            <div
            className="button btn btn-outline-primary output-button item-button" 
            data-toggle="button" 
            area-pressed="false" 
            autoComplete="off"
            >
                    <div><b>Output 1</b></div>
            
                    <div>
                        <span >Parot 1 : </span>
                        <span >0 0 0</span>
                    </div>  
                    <div>
                        <span >Port 2 : </span>
                        <span >0 0 0</span>
                    </div>
            </div>
        );

    }
}

export default OutputButton;