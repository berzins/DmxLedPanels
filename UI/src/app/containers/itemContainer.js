import React, { Component } from 'react';
import Fixture from './fixture'
import OutputButton from './outputButton'
import FixturePool from './fixturePool'
import OutputPool from './outputPool'

class ItemContainer extends Component {

    render() {
        return(
            <div>
            <div className="row">

                <div className="col-8">
                    <OutputPool/>
                </div>
                
                <div className="col-4">
                    <FixturePool/>
                </div>
                
            </div>
            </div>
        );
    }
}

export default ItemContainer;