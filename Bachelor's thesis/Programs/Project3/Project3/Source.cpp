#include <iostream>
#include "BackPackItem.h"
#include "Restrictions.h"

int limitation_sum_prices = 2000;
int limitation_sum_weights = 100;

void Gen_new_prices(std::vector<int>& prices)
{
	for (int i = 0; i < prices.size(); i++)
	{
		prices[i] = rand() % 5 + 1;
	}
}

void change_prices_to_spec_value_less_1(std::vector<int>& prices, std::vector<int>& weights)
{
	for (int i = 0; i < prices.size(); i++)
	{
		while (double(prices[i]) / weights[i] >= 1)
		{
			prices[i]--;
		}
	}

	for (int i = 0; i < prices.size(); i++)
	{
		while (double(prices[i]) / weights[i] <= 0.4)
		{
			prices[i]++;
		}
	}
}

int main()
	{
	setlocale(LC_ALL, "Russian");
	srand(time(NULL));

	//Вектор цен и веса
	std::vector<int> prices;
	std::vector<int> weights;
	//4 элемента 
	int elements = 5;
	//заполнение цены и веса
	for (int i = 0; i < elements; i++)
	{
		prices.push_back(rand() % 10 + 1);
		weights.push_back(rand() % 10 + 3);
	}
	change_prices_to_spec_value_less_1(prices, weights);
	change_prices_to_limit(prices);
	//change_weights_to_limit(weights);

		/*
		//сортировка spec_price_to_weight
		int buff_weight = 0; // для временного хранения значения, при перезаписи
		int buff_price = 0; // для временного хранения значения, при перезаписи
		for (int i = 0; i < elements - 1; i++) // 
		{
			for (int j = elements - 1; j > i; j--)
			{
				if (double(prices[j])/weights[j] > double(prices[j-1]) / weights[j-1])
				{
					buff_weight = weights[j - 1];
					weights[j - 1] = weights[j];
					weights[j] = buff_weight;

					buff_price = prices[j - 1];
					prices[j - 1] = prices[j];
					prices[j] = buff_price;
				}
			}
		}
		//корректировка price, чтобы набор был (1,..., 1) НЕ ОЧЕНЬ ПОМОГАЕТ
		for (int i = 0; i < elements - 1; i++)
		{
			while (double(prices[i]) / weights[i] > double(prices[i+1]) / weights[i+1])
			{
				prices[i]--;
			}
		}
		*/
	
	//перменные вес и цена равны начальным
	std::vector<int> tmp_prices;
	std::vector<int> tmp_weights;
	std::vector<int> res_appr_prices;
	std::vector<int> res_appr_weights;
	tmp_prices = prices;
	tmp_weights = weights;
	//нормировка максимального веса
	int max_weight = 0;
	for (int i = 0; i < elements; i++)
	{
		max_weight = max_weight + tmp_weights[i];
	}
	//нормировка цены 
	int max_price_for_set_with_all_1 = 0;
	for (int i = 0; i < elements; i++)
	{
		max_price_for_set_with_all_1 = max_price_for_set_with_all_1 + tmp_prices[i];
	}
	//рюкзак с переменными веса и цены
	BackPackItem B(tmp_prices, tmp_weights, elements);

	size_t j = 0;
	size_t k = 0;
	//сортировка цены в соответсвтии и веса по возрастанию
	sort_price_with_weight(tmp_prices, tmp_weights);
	bool flag = true;
	int result = 0;
	B.Method_dynamic_programming(max_weight);
	B.print();
	std::cout << "des: " << B.Method_dynamic_programming(max_weight) << std::endl;
	std::cout << "approx: " << B.Approximate_to_k_elements(max_weight, res_appr_prices, res_appr_weights) << std::endl << std::endl;
	//B.Method_dynamic_programming_find_set(max_weight);
	/*
	while(flag)
	{
		Gen_new_prices(tmp_prices);
		BackPackItem B_tmp(tmp_prices, tmp_weights, elements);
		if (B.Method_dynamic_programming(max_weight) == tmp_result_price)
		{
			flag = false;
		}
		r++;

		if (r == 100)
		{
			///
		}
		
	}
	BackPackItem B_new(tmp_prices, tmp_weights, elements);
	B.print();
	std::cout << std::endl << B.Method_dynamic_programming(max_weight) << " " << B.Approximate_kZero(max_weight);
	*/
	//

	/*
	/// ЛУЧШИЙ СЛУЧАЙ ДЛЯ alpha_2_4 ///
	tmp_prices[0] = 350;
	tmp_prices[1] = 1124;
	tmp_prices[2] = 2256;
	tmp_prices[3] = 6270;

	tmp_weights[0] = 40;
	tmp_weights[1] = 94;
	tmp_weights[2] = 145;
	tmp_weights[3] = 292;

	max_weight = 0;
	for (int i = 0; i < elements; i++)
	{
		max_weight = max_weight + tmp_weights[i];
	}
	/*--------------------------------*/
	
	
	/// ЛУЧШИЙ СЛУЧАЙ ДЛЯ alpha_2_5 ///
	tmp_prices[0] = 1;
	tmp_prices[1] = 9;
	tmp_prices[2] = 140;
	tmp_prices[3] = 255;
	tmp_prices[4] = 595;

	/*tmp_weights[0] = 2;
	tmp_weights[1] = 21;
	tmp_weights[2] = 157;
	tmp_weights[3] = 215;
	tmp_weights[4] = 396;*/
	tmp_weights[0] = 3;
	tmp_weights[1] = 51;
	tmp_weights[2] = 263;
	tmp_weights[3] = 449;
	tmp_weights[4] = 790;

	max_weight = 1556;
	/*--------------------------------*/

	/*
	/// ВМЕСТЕ С 200 - 225 ///
	std::cout << std::endl;
	std::cout << "Prices 0-4 indexes was: ";
	for (int i = 0; i < elements; i++)
	{
		std::cout << tmp_prices[i] << ", ";
	}
	std::cout << std::endl << "Weights 0-4 indexes was: ";
	for (int i = 0; i < elements; i++)
	{
		std::cout << tmp_weights[i] << ", ";
	}
	std::cout << std::endl;
	/*--------------------------------*/

	sort_price_with_weight(tmp_prices, tmp_weights);
	change_prices_to_limit(tmp_prices);

	//for (int i = 1; i < 10000; i++)
	//{
	//	sort_price_with_weight(tmp_prices, tmp_weights);

	//	BackPackItem B(tmp_prices, tmp_weights, elements);

	//	/*
	//	std::vector<int> pric = tmp_weights;
	//	std::vector<int> weig = tmp_weights;
	//	double appr = B.Approximate_to_k_elements(max_weight, pric, weig);
	//	double exact = B.Method_dynamic_programming(max_weight);
	//	double tmp = appr / exact;
	//	std::cout << "Prices: ";
	//	for (int i = 0; i < elements; i++)
	//	{
	//		std::cout << tmp_prices[i] << ", ";
	//	}
	//	std::cout << std::endl << "Weights: ";
	//	for (int i = 0; i < elements; i++)
	//	{
	//		std::cout << tmp_weights[i] << ", ";
	//	}
	//	std::cout << std::endl << "Appr prices : ";
	//	for (int i = 0; i < pric.size(); i++)
	//	{
	//		std::cout << pric[i] << ", ";
	//	}
	//	std::cout << std::endl << "Appr weights: ";
	//	for (int i = 0; i < weig.size(); i++)
	//	{
	//		std::cout << weig[i] << ", ";
	//	}
	//	std::cout << std::endl << "MaxWeight: " << max_weight << std::endl << tmp << " " << appr << " " << exact;
	//	*/

	//	result = B.Method_anneal(max_weight);
	//	std::cout << std::endl << std::endl;

	//	/// РАСКОММЕНТИРОВАТЬ, ЕСЛИ НЕ ИСПОЛЬЗОВАТЬ 142-152 СТРОКУ ///
	//	
	//	while (j < elements - (rand() % elements + 1))
	//	{
	//		tmp_prices[j] = tmp_prices[j] + rand() % 1000000 + 1;
	//		j = rand() % elements;
	//	}

	//	while (k < elements - (rand() % elements + 1))
	//	{
	//		tmp_weights[k] = tmp_weights[k] + rand() % 1000000 + 1;
	//		k = rand() % elements;
	//	}
	//	//change_prices_to_spec_value_less_1(tmp_prices, tmp_weights);
	//	

	//	change_prices_to_limit(tmp_prices);
	//	//change_weights_to_limit(tmp_weights);
	//}

	/// ЛУЧШИЙ СЛУЧАЙ ДЛЯ alpha_2_5 ///
	tmp_prices[0] = 1;
	tmp_prices[1] = 9;
	tmp_prices[2] = 140;
	tmp_prices[3] = 255;
	tmp_prices[4] = 595;

	tmp_weights[0] = 2;
	tmp_weights[1] = 21;
	tmp_weights[2] = 157;
	tmp_weights[3] = 215;
	tmp_weights[4] = 396;

	max_weight = 791;
	//change_prices_to_limit(tmp_prices);
	/*--------------------------------*/


	for (int i = 1; i < 10000; i++)
	{
		std::cout << i << "play) " << std::endl;
		sort_price_with_weight(tmp_prices, tmp_weights);

		BackPackItem B(tmp_prices, tmp_weights, elements);

		result = B.Method_anneal_2(max_weight);
		std::cout << std::endl << std::endl;

		/*
		while (j < elements - (rand() % elements + 1))
		{
			tmp_prices[j] = tmp_prices[j] + rand() % 2000 + 1;
			j = rand() % elements;
		}

		while (k < elements - (rand() % elements + 1))
		{
			tmp_weights[k] = tmp_weights[k] + rand() % 2000 + 1;
			k = rand() % elements;
		}
		*/

		//change_prices_to_limit(tmp_prices);
	}
	
}