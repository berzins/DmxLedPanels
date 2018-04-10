import React, { Component } from 'react';

class Fixture extends Component {

    render() {
        return(
            <div
            className="button btn btn-outline-secondary fixture-button item-button
            "
            data-toggle="button" 
            area-pressed="false" 
            autoComplete="off"
            >
                <div><b>Fixture Name</b></div>
                <div>
                    <div>
                        <span>Mode : </span>
                        <span >LW 3x3</span>
                    </div>
                    <div>
                        <span >Patch : </span>
                        <span >SCTL</span>
                    </div>
                    <div>
                        <span >Port : </span>
                        <span >0 0 1</span>
                    </div>
                    <div>
                        <span >Address : </span>
                        <span >81</span>
                    </div>
                </div>
            
            </div>
        );

    }
}

export default Fixture;