import * as React from 'react';
import { Helmet } from "react-helmet";

// Helmet.defaultProps.encodeSpecialCharacters = true;

class demo extends React.Component {


  render() {

    return (

      <div>
        <Helmet>
          <title>demo</title>
          <meta name="description" content="Nested componen111111111111111111t" />

        </Helmet>

        demo11111
      </div>
    );
  }
}

export default demo;
