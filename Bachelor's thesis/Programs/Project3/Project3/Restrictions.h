#pragma once

#include <iostream>
#include <vector>
#include <time.h>

#include <cmath>

#include <limits>
#include <iostream>
#include <cmath>
#include <algorithm>

extern int limitation_sum_prices;
extern int limitation_sum_weights;

int change_prices_to_limit(std::vector<int>& prices)
{
	int tmp_sum_prices = 0;
	for (int i = 0; i < prices.size(); i++)
	{
		tmp_sum_prices += prices[i];
	}

	int tmp_changed_value_price = 0;
	while (tmp_sum_prices != limitation_sum_prices)
	{
		int tmp_rand_index = rand() % prices.size();
		tmp_changed_value_price = rand() % limitation_sum_prices;

		if (tmp_sum_prices < limitation_sum_prices)
		{
			prices[tmp_rand_index] += tmp_changed_value_price;
			tmp_sum_prices += tmp_changed_value_price;
		}
		if (tmp_sum_prices > limitation_sum_prices)
		{
			if (prices[tmp_rand_index] > tmp_changed_value_price)
			{
				prices[tmp_rand_index] -= tmp_changed_value_price;
				tmp_sum_prices -= tmp_changed_value_price;
			}
		}
	}

	return tmp_sum_prices;
}

int change_weights_to_limit(std::vector<int>& weights)
{
	int tmp_sum_weights = 0;
	for (int i = 0; i < weights.size(); i++)
	{
		tmp_sum_weights += weights[i];
	}

	int tmp_changed_value_weight = 0;
	while (tmp_sum_weights != limitation_sum_weights)
	{
		int tmp_rand_index = rand() % weights.size();
		tmp_changed_value_weight = rand() % limitation_sum_weights;

		if (tmp_sum_weights < limitation_sum_weights)
		{
			weights[tmp_rand_index] += tmp_changed_value_weight;
			tmp_sum_weights += tmp_changed_value_weight;
		}
		if (tmp_sum_weights > limitation_sum_weights)
		{
			if (weights[tmp_rand_index] > tmp_changed_value_weight)
			{
				weights[tmp_rand_index] -= tmp_changed_value_weight;
				tmp_sum_weights -= tmp_changed_value_weight;
			}
		}
	}

	return tmp_sum_weights;
}