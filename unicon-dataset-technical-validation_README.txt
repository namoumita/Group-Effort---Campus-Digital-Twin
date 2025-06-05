### README for unicon-dataset-technical-validation.ipynb

#### Purpose:
This notebook performs technical validation on the dataset using Python and Pandas.

#### Markdown Overview:
# UNICON Dataset Technical Validation

This notebook presents a technical validation for selected subset of data in the UNICON dataset
### Load the needed libraries

#### Sample Code:
```python
# python
import datetime
from os.path import join, exists
from os import makedirs
import warnings
from datetime import datetime, timedelta

# data-science
import pandas as pd
import scipy
import numpy as np
import matplotlib.pyplot as plt

#Model building
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression
from sklearn import tree
from sklearn import neighbors
import xgboost as xgb
from sklearn.neural_network import MLPRegressor

from prettytable import PrettyTable
from sklearn import metrics 

%pylab inline
%matplotlib inline
plt.rcParams['figure.figsize'] = [18, 5]
warnings.filterwarnings('ignore')
warnings.simplefilter('ignore')
BASE_PATH = "../input/unicon/"
```
